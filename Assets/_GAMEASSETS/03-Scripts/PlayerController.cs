using System;
using System.IO.MemoryMappedFiles;
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
        [SerializeField] private LayerMask WhatCanBePushed;

        [SerializeField] private GameObject playerSpriteGO;
        private Animator anim;
        [SerializeField] private bool canMove = true;
        
        private HistoryManager history;
        private SettingsManager settings;
        private Vector2 rawAxis;

        private void Start()
        {
            anim = playerSpriteGO.GetComponent<Animator>();
            movePoint.parent = null;
            rawAxis = Vector2.zero;
            history = GetComponent<HistoryManager>();
            settings = GameObject.FindGameObjectWithTag(GameTags.SETTINGSMANAGER).GetComponent<SettingsManager>();
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
            
            if (!animateInverted)
            {
                //Set Animations
                switch (direction)
                {
                    case MoveDirections.Right:
                    {
                        anim.SetFloat("horizontal", 1f);
                        anim.SetFloat("vertical", 0f);
                        break;
                    }
                    case MoveDirections.Left:
                    {
                        anim.SetFloat("horizontal", -1f);
                        anim.SetFloat("vertical", 0f);
                        break;
                    }
                    case MoveDirections.Up:
                    {
                        anim.SetFloat("horizontal", 0f);
                        anim.SetFloat("vertical", 1f);
                        break;
                    }
                    case MoveDirections.Down:
                    {
                        anim.SetFloat("horizontal", 0f);
                        anim.SetFloat("vertical", -1f);
                        break;
                    }
                    default:
                    {
                        anim.SetFloat("horizontal", 0f);
                        anim.SetFloat("vertical", 0f);
                        break;
                    }
                }
            }
            else
            {
                //Set Animations Reversed
                switch (direction)
                {
                    case MoveDirections.Right:
                    {
                        anim.SetFloat("horizontal", -1f);
                        anim.SetFloat("vertical", 0f);
                        break;
                    }
                    case MoveDirections.Left:
                    {
                        anim.SetFloat("horizontal", 1f);
                        anim.SetFloat("vertical", 0f);
                        break;
                    }
                    case MoveDirections.Up:
                    {
                        anim.SetFloat("horizontal", 0f);
                        anim.SetFloat("vertical", -1f);
                        break;
                    }
                    case MoveDirections.Down:
                    {
                        anim.SetFloat("horizontal", 0f);
                        anim.SetFloat("vertical", 1f);
                        break;
                    }
                    default:
                    {
                        anim.SetFloat("horizontal", 0f);
                        anim.SetFloat("vertical", 0f);
                        break;
                    }
                }
            }

            //RIGHT MOVEMENT
            if (direction == MoveDirections.Right)
            {
                if (Physics2D.OverlapCircle(movePoint.position + new Vector3(1f, 0f),
                    .2f, WhatStopsMovement))
                {
                    //DO NOTHING
                }
                else if (Physics2D.OverlapCircle(movePoint.position + new Vector3(1f, 0f), .2f, WhatCanBePushed))
                {
                    GameObject pushedObject = Physics2D.OverlapCircle(movePoint.position + new Vector3(1f, 0f), .2f, WhatCanBePushed).gameObject;
                    PushBlock(pushedObject, Vector2.right);
                }
                else
                {
                    movePoint.position += new Vector3(1f, 0f);
                    if (saveHistory)
                    {
                        history.addMove(MoveTypes.MOVERIGHT);
                    }
                }
            }
            //LEFT MOVEMENT
            else if (direction == MoveDirections.Left)
            {
                if (Physics2D.OverlapCircle(movePoint.position + new Vector3(-1f, 0f), .2f, WhatStopsMovement))
                {
                    //DO NOTHING
                }
                else if (Physics2D.OverlapCircle(
                    movePoint.position + new Vector3(-1f, 0f), .2f, WhatCanBePushed))
                {
                    GameObject pushedObject = Physics2D.OverlapCircle(movePoint.position + new Vector3(-1f, 0f), .2f, WhatCanBePushed).gameObject;

                    PushBlock(pushedObject, Vector2.left);
                }
                else
                {
                    movePoint.position += new Vector3(-1f, 0f);
                    if (saveHistory)
                    {
                        history.addMove(MoveTypes.MOVELEFT);
                    }
                }
            }

            //UP MOVEMENT
            else if (direction == MoveDirections.Up)
            {
                if (Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, 1f), .2f,
                    WhatStopsMovement))
                {
                    //DO NOTHING
                }
                else if (Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, 1f),
                    .2f, WhatCanBePushed))
                {
                    GameObject pushedObject = Physics2D.OverlapCircle(
                            movePoint.position + new Vector3(0f, 1f), .2f,
                            WhatCanBePushed)
                        .gameObject;

                    PushBlock(pushedObject, Vector2.up);
                }
                else
                {
                    movePoint.position += new Vector3(0f, 1f);
                    if (saveHistory)
                    {
                        history.addMove(MoveTypes.MOVEUP);
                    }
                }
            }
            //DOWN MOVEMENT
            else if (direction == MoveDirections.Down)
            {
                if (Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, -1f), .2f,
                    WhatStopsMovement))
                {
                    //DO NOTHING
                }
                else if (Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, -1f),
                    .2f, WhatCanBePushed))
                {
                    GameObject pushedObject = Physics2D.OverlapCircle(
                            movePoint.position + new Vector3(0f, -1f), .2f,
                            WhatCanBePushed)
                        .gameObject;

                    PushBlock(pushedObject, Vector2.down);
                }
                else
                {
                    movePoint.position += new Vector3(0f, -1f);
                    if (saveHistory)
                    {
                        history.addMove(MoveTypes.MOVEDOWN);
                    }
                }
            }
        }

        private void PushBlock(GameObject blockToPush, Vector2 direction)
        {
            if (blockToPush.GetComponent<IPushable>().Push(direction, moveSpeed))
            {
                if (direction == Vector2.down)
                {
                    history.addMove(MoveTypes.PUSHDOWN);
                    movePoint.position += new Vector3(direction.x, direction.y);
                }
                if (direction == Vector2.up)
                {
                    history.addMove(MoveTypes.PUSHUP);
                    movePoint.position += new Vector3(direction.x, direction.y);
                }
                if (direction == Vector2.left)
                {
                    history.addMove(MoveTypes.PUSHLEFT);
                    movePoint.position += new Vector3(direction.x, direction.y);
                }
                if (direction == Vector2.right)
                {
                    history.addMove(MoveTypes.PUSHRIGHT);
                    movePoint.position += new Vector3(direction.x, direction.y);
                }
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
                    GameObject pushedObject = Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, 1f), .2f, WhatCanBePushed).gameObject;
                    Move(MoveDirections.Down, true, false);
                    IPushable pushable = pushedObject.GetComponent<IPushable>();
                    pushable.UndoPush();
                    break;
                }
                case MoveTypes.PUSHDOWN:
                {
                    GameObject pushedObject = Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, -1f), .2f, WhatCanBePushed).gameObject;
                    Move(MoveDirections.Up, true, false);
                    IPushable pushable = pushedObject.GetComponent<IPushable>();
                    pushable.UndoPush();
                    break;
                }
                case MoveTypes.PUSHLEFT:
                {
                    GameObject pushedObject = Physics2D.OverlapCircle(movePoint.position + new Vector3(-1f, 0f), .2f, WhatCanBePushed).gameObject;
                    Move(MoveDirections.Right, true, false);
                    IPushable pushable = pushedObject.GetComponent<IPushable>();
                    pushable.UndoPush();
                    break;
                }
                case MoveTypes.PUSHRIGHT:
                {
                    GameObject pushedObject = Physics2D.OverlapCircle(movePoint.position + new Vector3(1f, 0f), .2f, WhatCanBePushed).gameObject;
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

        public void BTN_Pause(InputAction.CallbackContext ctx)
        {
            //pauseMenu.PauseGame();
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
            Pause
        }
    }
}