using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace LittleSubmarine2
{
    public class PlayerController : MonoBehaviour, IMovable
    {
        [Range(0.0f, 10.0f)] [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private GameObject arrowButtons;
        [SerializeField] private Transform movePoint;

        [SerializeField] private GameObject playerSpriteGO;
        [SerializeField] private SpriteRenderer playerBodySprite;
        [SerializeField] private SpriteRenderer playerPeriscopeSprite;

        [SerializeField] private Collider2D selfCollider;
        
        [SerializeField] private bool canMove = true;

        [SerializeField] private TMP_Text moveCounterText;
        [SerializeField] private int usedMoves;
        private DateTime startTime;

        private HistoryManager history;
        private SettingsManager settings;
        private PartManager partManager;
        private SaveManager saveManager;
        private Vector2 rawAxis;
        private Animator anim;

        public int UsedMoves => usedMoves;
        public DateTime StartTime => startTime;

        public Transform MovePoint
        {
            get => movePoint;
            set => movePoint = value;
        }

        public Collider2D SelfCollider
        {
            get => selfCollider;
            set => selfCollider = value;
        }

        public HistoryManager History
        {
            get => history;
            set => history = value;
        }

        public Animator Anim
        {
            get => anim;
            set => anim = value;
        }

        private void Start()
        {
            //Get references to managers:
            settings = GameObject.FindGameObjectWithTag(GameTags.SETTINGSMANAGER).GetComponent<SettingsManager>();
            partManager = GameObject.FindGameObjectWithTag(GameTags.PARTMANAGER).GetComponent<PartManager>();
            saveManager = GameObject.FindGameObjectWithTag(GameTags.SAVEMANAGER).GetComponent<SaveManager>();

            //Get references to Components:
            anim = playerSpriteGO.GetComponent<Animator>();
            history = GetComponent<HistoryManager>();
            SelfCollider = GetComponent<Collider2D>();
            
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
                    Move(rawAxis, true);

                    anim.SetBool("moving", false);
                }
                else
                {
                    anim.SetBool("moving", true);
                }
            }
        }

        private void Move(Vector2 direction, bool forwards)
        {
            if (canMove)
            {
                if (forwards && direction != Vector2.zero)
                {
                    if (Physics2D.OverlapCircle(movePoint.position + new Vector3(direction.x, direction.y), .2f, LayerTypes.SOLID))
                    {
                        //do nothing if movement is blocked
                    }
                    else if (Physics2D.OverlapCircle(movePoint.position + new Vector3(direction.x, direction.y), .2f, LayerTypes.SPECIALTILES))
                    {
                        SpecialTile tile = Physics2D.OverlapCircle(movePoint.position + new Vector3(direction.x, direction.y), .2f, LayerTypes.SPECIALTILES)
                            .GetComponent<SpecialTile>();
                        if (TileHelper.CanUseSpecialTile(tile, direction, true, this))
                        {
                            usedMoves += 1;
                        }
                    }
                    else
                    {
                        Command move = new Move(this, direction);
                        if (move.Execute())
                        {
                            usedMoves += 1;
                        }
                    }
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
            Command lastMove = history.getUndoMove();
            if (lastMove != null)
            {
                if (lastMove.Undo())
                {
                    usedMoves -= 1;
                }
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
                    Debug.Log("Swipe down detected!");
                    Move(Vector2.down, true); 
                    break;
                case Swipe.Up:
                    Debug.Log("Swipe up detected!");
                    Move(Vector2.up,true); 
                    break;
                case Swipe.Left:
                    Debug.Log("Swipe left detected!");
                    Move(Vector2.left, true); 
                    break;
                case Swipe.Right:
                    Debug.Log("Swipe right detected!");
                    Move(Vector2.right, true); 
                    break;
            }
        }
    }
}