using System;
using System.IO.MemoryMappedFiles;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace LittleSubmarine2
{
    public class PlayerController : MonoBehaviour
    {
        [Range(0.0f, 10.0f)] [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private GameObject arrowButtons;
        [SerializeField] private Transform movePoint;

        [SerializeField] private LayerMask WhatStopsMovement;
        [SerializeField] private LayerMask SpecialTiles;
        [SerializeField] private GameObject playerSpriteGO;
        [SerializeField] private SpriteRenderer playerBodySprite;
        [SerializeField] private SpriteRenderer playerPeriscopeSprite;
        
        [SerializeField] private bool canMove = true;

        [SerializeField] private TMP_Text moveCounterText;
        [SerializeField] private int usedMoves;
        [SerializeField] private DateTime startTime;

        private HistoryManager history;
        private SettingsManager settings;
        private PartManager partManager;
        private SaveManager saveManager;
        private Vector2 rawAxis;
        private Animator anim;

        private MoveDirections buttonHoldingDirection;

        public int UsedMoves => usedMoves;
        public DateTime StartTime => startTime;

        private void Start()
        {
            //Get references to managers:
            settings = GameObject.FindGameObjectWithTag(GameTags.SETTINGSMANAGER).GetComponent<SettingsManager>();
            partManager = GameObject.FindGameObjectWithTag(GameTags.PARTMANAGER).GetComponent<PartManager>();
            saveManager = GameObject.FindGameObjectWithTag(GameTags.SAVEMANAGER).GetComponent<SaveManager>();

            //Get references to Components:
            anim = playerSpriteGO.GetComponent<Animator>();
            history = GetComponent<HistoryManager>();
            
            //Gets the selected body parts:
            playerBodySprite.sprite = partManager.GetBodyByID(saveManager.GetData().selectedBody).SpriteImage;
            playerPeriscopeSprite.sprite = partManager.GetPeriscopeByID(saveManager.GetData().selectedPeriscope).SpriteImage;
            
            //Initialize Positions and parents:
            movePoint.parent = null;
            rawAxis = Vector2.zero;
            startTime = DateTime.Now;
            
            //Update arrow buttons according to settings:
            UpdateArrowButtons();
        }

        private void OnEnable()
        {
            SwipeManager.OnSwipeDetected += OnSwipeDetected;
        }

        private void OnDisable()
        {
            SwipeManager.OnSwipeDetected -= OnSwipeDetected;
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
            {
                if (canMove)
                {
                    //RIGHT MOVEMENT
                    if (rawAxis.x == 1f)
                    {
                        Move(MoveDirections.Right, false, true);
                    }
                    //LEFT MOVEMENT
                    else if (rawAxis.x == -1f)
                    {
                        Move(MoveDirections.Left, false, true);
                    }

                    //UP MOVEMENT
                    else if (rawAxis.y == 1f)
                    {
                        Move(MoveDirections.Up, false, true);
                    }
                    //DOWN MOVEMENT
                    else if (rawAxis.y == -1f)
                    {
                        Move(MoveDirections.Down, false, true);
                    }

                    anim.SetBool("moving", false);
                }
                else
                {
                    anim.SetBool("moving", true);
                }
            }
        }

        private void Move(MoveDirections direction, bool animateInverted, bool saveHistory)
        {
            Vector2 directionToMove;
            MoveTypes moveType;
            
            if (canMove)
            {

                switch (direction)
                {
                    default:
                        directionToMove = Vector2.zero;
                        moveType = MoveTypes.EMPTY;
                        break;
                    case MoveDirections.Right:
                        directionToMove = Vector2.right;
                        moveType = MoveTypes.MOVERIGHT;
                        break;
                    case MoveDirections.Left:
                        directionToMove = Vector2.left;
                        moveType = MoveTypes.MOVELEFT;
                        break;
                    case MoveDirections.Up:
                        directionToMove = Vector2.up;
                        moveType = MoveTypes.MOVEUP;
                        break;
                    case MoveDirections.Down:
                        directionToMove = Vector2.down;
                        moveType = MoveTypes.MOVEDOWN;
                        break;
                }
                
                //SET ANIMATIONS
                if (!animateInverted)
                {
                    anim.SetFloat("horizontal", directionToMove.x);
                    anim.SetFloat("vertical", directionToMove.y);
                }
                else
                {
                    anim.SetFloat("horizontal", -directionToMove.x);
                    anim.SetFloat("vertical", -directionToMove.y);
                }
                
                //MOVE
                if (Physics2D.OverlapCircle(movePoint.position + new Vector3(directionToMove.x,directionToMove.y), .2f, WhatStopsMovement))
                {
                    //do nothing if movement is blocked
                }
                else if(Physics2D.OverlapCircle(movePoint.position + new Vector3(directionToMove.x,directionToMove.y), .2f, SpecialTiles))
                {
                    SpecialTile tile = Physics2D.OverlapCircle(movePoint.position + new Vector3(directionToMove.x, directionToMove.y), .2f, SpecialTiles).GetComponent<SpecialTile>();
                    if (CanUseSpecialTile(tile, directionToMove, moveType, saveHistory))
                    {
                        movePoint.position += new Vector3(directionToMove.x,directionToMove.y);
                        usedMoves += 1;
                    }
                }
                else
                {
                    movePoint.position += new Vector3(directionToMove.x,directionToMove.y);
                    usedMoves += 1;
                    if (saveHistory)
                    {
                        history.addMove(moveType);
                    }
                }
            }
            
            moveCounterText.text = usedMoves + " Moves";
        }

        private bool CanUseSpecialTile(SpecialTile tileIn, Vector2 direction, MoveTypes moveType, bool saveHistory)
        {
            switch (tileIn.GetTileType())
            {
                default:
                    return true;
                    history.addMove(moveType);
                case SpecialTileTypes.ONEWAY:
                    Oneway ow = tileIn.GetComponent<Oneway>();
                    history.addMove(moveType);
                    return direction == ow.GetDirection();
                case SpecialTileTypes.PUSHABLE:
                    GameObject pushedObject = tileIn.gameObject;
                    if (PushBlock(pushedObject, direction))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
        }

        private bool PushBlock(GameObject blockToPush, Vector2 direction)
        {
            if (blockToPush.GetComponent<IPushable>().Push(direction, moveSpeed))
            {
                if (direction == Vector2.down)
                {
                    history.addMove(MoveTypes.PUSHDOWN);
                }
                if (direction == Vector2.up)
                {
                    history.addMove(MoveTypes.PUSHUP);
                }
                if (direction == Vector2.left)
                {
                    history.addMove(MoveTypes.PUSHLEFT);
                }
                if (direction == Vector2.right)
                {
                    history.addMove(MoveTypes.PUSHRIGHT);
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetCanMove(bool canMoveIn)
        {
            canMove = canMoveIn;
        }

        private void UndoMovement()
        {
            MoveTypes lastMove = history.getUndoMove();

            switch (lastMove)
            {
                case MoveTypes.MOVEUP:
                {
                    Move(MoveDirections.Down, true, false);
                    break;
                }
                case MoveTypes.MOVEDOWN:
                {
                    Move(MoveDirections.Up, true, false);
                    break;
                }
                case MoveTypes.MOVELEFT:
                {
                    Move(MoveDirections.Right, true, false);
                    break;
                }
                case MoveTypes.MOVERIGHT:
                {
                    Move(MoveDirections.Left, true, false);
                    break;
                }
                
                case MoveTypes.PUSHUP:
                {
                    GameObject pushedObject = Physics2D.OverlapCircle((Vector2)movePoint.position + Vector2.up, .2f, SpecialTiles).gameObject;
                    Move(MoveDirections.Down, true, false);
                    IPushable pushable = pushedObject.GetComponent<IPushable>();
                    pushable.UndoPush();
                    break;
                }
                case MoveTypes.PUSHDOWN:
                {
                    GameObject pushedObject = Physics2D.OverlapCircle((Vector2)movePoint.position + Vector2.down, .2f, SpecialTiles).gameObject;
                    Move(MoveDirections.Up, true, false);
                    IPushable pushable = pushedObject.GetComponent<IPushable>();
                    pushable.UndoPush();
                    break;
                }
                case MoveTypes.PUSHLEFT:
                {
                    GameObject pushedObject = Physics2D.OverlapCircle((Vector2)movePoint.position + Vector2.left, .2f, SpecialTiles).gameObject;
                    Move(MoveDirections.Right, true, false);
                    IPushable pushable = pushedObject.GetComponent<IPushable>();
                    pushable.UndoPush();
                    break;
                }
                case MoveTypes.PUSHRIGHT:
                {
                    GameObject pushedObject = Physics2D.OverlapCircle((Vector2)movePoint.position + Vector2.right, .2f, SpecialTiles).gameObject;
                    Move(MoveDirections.Left, true, false);
                    IPushable pushable = pushedObject.GetComponent<IPushable>();
                    pushable.UndoPush();
                    break;
                }
                default:
                {
                    break;
                }
            }

            if (usedMoves != 0)
            {
                usedMoves -= 2;
                moveCounterText.text = usedMoves + " Moves";
            }
        }

        public void UpdateArrowButtons()
        {
            if (settings.ShowSteeringWheel)
            {
                arrowButtons.SetActive(true);
            }
            else
            {
                arrowButtons.SetActive(false);
            }
        }

        public void BTN_DirectionUpHold()
        {
            if (canMove)
            {
                rawAxis = new Vector2(0, 1);
            }
        }
        
        public void BTN_DirectionDownHold()
        {
            if (canMove)
            {
                rawAxis = new Vector2(0, -1);
            }
        }
        
        public void BTN_DirectionLeftHold()
        {
            if (canMove)
            {
                rawAxis = new Vector2(-1, 0);
            }
        }
        
        public void BTN_DirectionRightHold()
        {
            if (canMove)
            {
                rawAxis = new Vector2(1, 0);
            }
        }

        public void BTN_DirectionRelease()
        {
            rawAxis = Vector2.zero;
        }

        public void BTN_Up()
        {
            if (canMove)
            {
                Move(MoveDirections.Up, false,true);
            }
        }
        public void BTN_Down()
        {
            if (canMove)
            {
                Move(MoveDirections.Down, false,true);
            }
        }
        public void BTN_Left()
        {
            if (canMove)
            {
                Move(MoveDirections.Left, false,true);
            }
        }
        public void BTN_Right()
        {
            if (canMove)
            {
                Move(MoveDirections.Right, false,true);
            }
        }

        public void BTN_Undo(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                if (canMove)
                {
                    UndoMovement();
                }
            }
        }
        
        public void BTN_Undo()
        {
            if (canMove)
            {
                UndoMovement();
            }
        }

        public void BTN_BackToMenu()
        {
            SceneManager.LoadScene(Scenes.LEVELOVERVIEW);
        }

        public void BTN_Move(InputAction.CallbackContext ctx)
        {
            if (canMove)
            {
                rawAxis = ctx.ReadValue<Vector2>();
            }
        }
        
        public void OnEscape(InputAction.CallbackContext ctx)
        {
            BTN_BackToMenu();
        }

        public void OnSwipeDetected (Swipe direction, Vector2 swipeVelocity)
        {
            switch (direction)
            {
                case Swipe.Down:
                    Move(MoveDirections.Down, false,true); 
                    break;
                case Swipe.Up:
                    Move(MoveDirections.Up, false,true); 
                    break;
                case Swipe.Left:
                    Move(MoveDirections.Left, false,true); 
                    break;
                case Swipe.Right:
                    Move(MoveDirections.Right, false,true); 
                    break;
            }
        }

        public enum MoveDirections
        {
            Right,
            Left,
            Up,
            Down,
            None,
            Pause
        }
    }
}