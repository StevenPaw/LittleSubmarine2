using System;
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

        [SerializeField] private LayerMask whatStopsMovement;
        [SerializeField] private LayerMask specialTiles;
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
        private Collider2D selfCollider;

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
            selfCollider = GetComponent<Collider2D>();
            
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

                bool canLeaveTile = true;
                
                if (Physics2D.OverlapCircle((Vector2) movePoint.position, .2f, specialTiles))
                {
                    canLeaveTile = TileHelper.CanLeaveSpecialTile(selfCollider, directionToMove, specialTiles);
                }
                
                //MOVE
                if (canLeaveTile)
                {
                    if (Physics2D.OverlapCircle(movePoint.position + new Vector3(directionToMove.x, directionToMove.y), .2f, whatStopsMovement))
                    {
                        //do nothing if movement is blocked
                    }
                    else if (Physics2D.OverlapCircle(movePoint.position + new Vector3(directionToMove.x, directionToMove.y), .2f, specialTiles))
                    {
                        SpecialTile tile = Physics2D.OverlapCircle(movePoint.position + new Vector3(directionToMove.x, directionToMove.y), .2f, specialTiles)
                            .GetComponent<SpecialTile>();
                        if (TileHelper.CanUseSpecialTile(tile, directionToMove, moveType, saveHistory, moveSpeed, history))
                        {
                            movePoint.position += new Vector3(directionToMove.x, directionToMove.y);
                            usedMoves += 1;
                        }
                    }
                    else
                    {
                        movePoint.position += new Vector3(directionToMove.x, directionToMove.y);
                        usedMoves += 1;
                        if (saveHistory)
                        {
                            history.addMove(moveType);
                        }
                    }
                }
            }
            
            moveCounterText.text = usedMoves + " Moves";
        }
        
        private void MoveBack(MoveDirections direction, bool animateInverted)
        {
            Vector2 directionToMove;
            
            if (canMove)
            {
                switch (direction)
                {
                    default:
                        directionToMove = Vector2.zero;
                        break;
                    case MoveDirections.Right:
                        directionToMove = Vector2.right;
                        break;
                    case MoveDirections.Left:
                        directionToMove = Vector2.left;
                        break;
                    case MoveDirections.Up:
                        directionToMove = Vector2.up;
                        break;
                    case MoveDirections.Down:
                        directionToMove = Vector2.down;
                        break;
                }
                
                //SET ANIMATIONS
                anim.SetFloat("horizontal", -directionToMove.x);
                anim.SetFloat("vertical", -directionToMove.y);
                
                //MOVE
                if (Physics2D.OverlapCircle(movePoint.position + new Vector3(directionToMove.x, directionToMove.y), .2f, whatStopsMovement))
                {
                    //do nothing if movement is blocked
                }
                else if (Physics2D.OverlapCircle(movePoint.position + new Vector3(directionToMove.x, directionToMove.y), .2f, specialTiles))
                {
                    movePoint.position += new Vector3(directionToMove.x, directionToMove.y);
                    usedMoves -= 1;
                }
                else
                {
                    movePoint.position += new Vector3(directionToMove.x, directionToMove.y);
                    usedMoves -= 1;
                }
            }
            
            moveCounterText.text = usedMoves + " Moves";
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
                    MoveBack(MoveDirections.Down, true);
                    break;
                }
                case MoveTypes.MOVEDOWN:
                {
                    MoveBack(MoveDirections.Up, true);
                    break;
                }
                case MoveTypes.MOVELEFT:
                {
                    MoveBack(MoveDirections.Right, true);
                    break;
                }
                case MoveTypes.MOVERIGHT:
                {
                    MoveBack(MoveDirections.Left, true);
                    break;
                }
                
                case MoveTypes.PUSHUP:
                {
                    GameObject pushedObject = Physics2D.OverlapCircle((Vector2)movePoint.position + Vector2.up, .2f, specialTiles).gameObject;
                    MoveBack(MoveDirections.Down, true);
                    IPushable pushable = pushedObject.GetComponent<IPushable>();
                    pushable.UndoPush();
                    break;
                }
                case MoveTypes.PUSHDOWN:
                {
                    GameObject pushedObject = Physics2D.OverlapCircle((Vector2)movePoint.position + Vector2.down, .2f, specialTiles).gameObject;
                    MoveBack(MoveDirections.Up, true);
                    IPushable pushable = pushedObject.GetComponent<IPushable>();
                    pushable.UndoPush();
                    break;
                }
                case MoveTypes.PUSHLEFT:
                {
                    GameObject pushedObject = Physics2D.OverlapCircle((Vector2)movePoint.position + Vector2.left, .2f, specialTiles).gameObject;
                    MoveBack(MoveDirections.Right, true);
                    IPushable pushable = pushedObject.GetComponent<IPushable>();
                    pushable.UndoPush();
                    break;
                }
                case MoveTypes.PUSHRIGHT:
                {
                    GameObject pushedObject = Physics2D.OverlapCircle((Vector2)movePoint.position + Vector2.right, .2f, specialTiles).gameObject;
                    MoveBack(MoveDirections.Left, true);
                    IPushable pushable = pushedObject.GetComponent<IPushable>();
                    pushable.UndoPush();
                    break;
                }
            }

            if (usedMoves != 0)
            {
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