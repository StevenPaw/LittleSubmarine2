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
        
        

        [SerializeField] private TMP_Text moveCounterText;
        [SerializeField] private int usedMoves;
        private DateTime startTime;

        private HistoryManager history;
        private SettingsManager settings;
        private PartManager partManager;
        private SaveManager saveManager;
        private Animator anim;
        private PlayerInputManager playerInputManager;

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

        private void OnEnable()
        {
            SwipeManager.OnSwipeDetected += OnSwipeDetected;
        }

        private void OnDisable()
        {
            SwipeManager.OnSwipeDetected -= OnSwipeDetected;
        }

        private void Start()
        {
            //Get references to managers:
            settings = SettingsManager.Instance;
            partManager = PartManager.Instance;
            saveManager = SaveManager.Instance;
            playerInputManager = PlayerInputManager.Instance;

            //Get references to Components:
            anim = playerSpriteGO.GetComponent<Animator>();
            history = GetComponent<HistoryManager>();
            SelfCollider = GetComponent<Collider2D>();
            
            //Gets the selected body parts:
            playerBodySprite.sprite = partManager.GetBodyByID(saveManager.GetData().selectedBody).SpriteImage;
            playerPeriscopeSprite.sprite = partManager.GetPeriscopeByID(saveManager.GetData().selectedPeriscope).SpriteImage;
            
            //Initialize Positions and parents:
            movePoint.parent = null;
            PlayerInputManager.Instance.RawAxis = Vector2.zero;
            startTime = DateTime.Now;
            
            //Update arrow buttons according to settings:
            UpdateArrowButtons();
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, movePoint.position) <= .05f)
            {
                if (playerInputManager.CanMove)
                {
                    Move(playerInputManager.RawAxis, true);

                    anim.SetBool("moving", false);
                }
                else
                {
                    anim.SetBool("moving", true);
                }
            }
        }

        private void Move(Vector2 direction, bool timeDirectionForwards)
        {
            if (playerInputManager.CanMove)
            {
                if (timeDirectionForwards && direction != Vector2.zero)
                {
                    if (Physics2D.OverlapCircle(movePoint.position + new Vector3(direction.x, direction.y), .2f, LayerTypes.SOLID))
                    {
                        return; //Do nothing if way is blocked
                    }
                    
                    if (Physics2D.OverlapCircle(movePoint.position + new Vector3(direction.x, direction.y), .2f, LayerTypes.SPECIALTILES))
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

        public void BTN_Undo(InputAction.CallbackContext ctx)
        {
            if (ctx.started)
            {
                if (playerInputManager.CanMove)
                {
                    UndoMovement();
                }
            }
        }
        
        public void BTN_Undo()
        {
            if (playerInputManager.CanMove)
            {
                UndoMovement();
            }
        }

        public void OnSwipeDetected (Swipe direction, Vector2 swipeVelocity)
        {
            switch (direction)
            {
                case Swipe.Down:
                    Move(Vector2.down, true); 
                    break;
                case Swipe.Up:
                    Move(Vector2.up,true); 
                    break;
                case Swipe.Left:
                    Move(Vector2.left, true); 
                    break;
                case Swipe.Right:
                    Move(Vector2.right, true); 
                    break;
            }
        }
    }
}