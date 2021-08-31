using System.IO.MemoryMappedFiles;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Range(0.0f,10.0f)] [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Transform movePoint;

    [SerializeField] private LayerMask WhatStopsMovement;
    [SerializeField] private LayerMask WhatCanBePushed;

    [SerializeField] private Animator anim;

    [SerializeField] private KeyCode undoKey;

    [SerializeField] private bool canMove = true;

    private Vector2 rawAxis;
    
    private void Start()
    {
        movePoint.parent = null;
        rawAxis = Vector2.zero;
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
                    Move(MoveDirections.Right);
                }
                //LEFT MOVEMENT
                else if (rawAxis.x == -1f)
                {
                    Move(MoveDirections.Left);
                }

                //UP MOVEMENT
                else if (rawAxis.y == 1f)
                {
                    Move(MoveDirections.Up);
                }
                //DOWN MOVEMENT
                else if (rawAxis.y == -1f)
                {
                    Move(MoveDirections.Down);
                }

                anim.SetBool("moving", false);
            }
            else
            {
                anim.SetBool("moving", true);
            }
        }
    }
    
    public void Move(MoveDirections direction)
    {
        //Set Animations
        switch (direction)
        {
            case MoveDirections.Right:
            {
                anim.SetFloat("Horizontal", 1f);
                anim.SetFloat("Vertical", 0f);
                break;
            }
            case MoveDirections.Left:
            {
                anim.SetFloat("Horizontal", -1f);
                anim.SetFloat("Vertical", 0f);
                break;
            }
            case MoveDirections.Up:
            {
                anim.SetFloat("Horizontal", 0f);
                anim.SetFloat("Vertical", 1f);
                break;
            }
            case MoveDirections.Down:
            {
                anim.SetFloat("Horizontal", 0f);
                anim.SetFloat("Vertical", -1f);
                break;
            }
            default:
            {
                anim.SetFloat("Horizontal", 0f);
                anim.SetFloat("Vertical", 0f);
                break;
            }
        }
        
        //RIGHT MOVEMENT
        if (direction == MoveDirections.Right)
        {
            if (Physics2D.OverlapCircle(movePoint.position + new Vector3(rawAxis.x, 0f),
                .2f, WhatStopsMovement))
            {
                //DO NOTHING
            }
            else if (Physics2D.OverlapCircle(
                movePoint.position + new Vector3(rawAxis.x, 0f), .2f, WhatCanBePushed))
            {
                GameObject pushedObject = Physics2D.OverlapCircle(
                        movePoint.position + new Vector3(rawAxis.x, 0f), .2f,
                        WhatCanBePushed)
                    .gameObject;

                PushBlock(pushedObject, Vector2.right);
            }
            else
            {
                movePoint.position += new Vector3(rawAxis.x, 0f);
                // if (undoManager.addMove(UndoManager.MovementType.RIGHT))
                // {
                //     movePoint.position += new Vector3(rawAxis.x, 0f);
                // }
                // else
                // {
                //     messengerManager.MessageTooManyMoves();
                // }
            }
        }
        //LEFT MOVEMENT
        else if (direction == MoveDirections.Left)
        {
            if (Physics2D.OverlapCircle(movePoint.position + new Vector3(rawAxis.x, 0f), .2f, WhatStopsMovement))
            {
                //DO NOTHING
            }
            else if (Physics2D.OverlapCircle(
                movePoint.position + new Vector3(rawAxis.x, 0f), .2f, WhatCanBePushed))
            {
                GameObject pushedObject = Physics2D.OverlapCircle(
                        movePoint.position + new Vector3(rawAxis.x, 0f), .2f, WhatCanBePushed).gameObject;

                PushBlock(pushedObject, Vector2.left);
            }
            else
            {
                movePoint.position += new Vector3(rawAxis.x, 0f);
                // if (undoManager.addMove(UndoManager.MovementType.LEFT))
                // {
                //     movePoint.position += new Vector3(rawAxis.x, 0f);
                // }
                // else
                // {
                //     messengerManager.MessageTooManyMoves();
                // }
            }
        }

        //UP MOVEMENT
        else if (direction == MoveDirections.Up)
        {
            if (Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, rawAxis.y), .2f,
                WhatStopsMovement))
            {
                //DO NOTHING
            }
            else if (Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, rawAxis.y),
                .2f, WhatCanBePushed))
            {
                GameObject pushedObject = Physics2D.OverlapCircle(
                        movePoint.position + new Vector3(0f, rawAxis.y), .2f,
                        WhatCanBePushed)
                    .gameObject;

                PushBlock(pushedObject, Vector2.up);
            }
            else
            {
                movePoint.position += new Vector3(0f, rawAxis.y);
                // if (undoManager.addMove(UndoManager.MovementType.UP))
                // {
                //     
                // }
                // else
                // {
                //     messengerManager.MessageTooManyMoves();
                // }
            }
        }
        //DOWN MOVEMENT
        else if (rawAxis.y == -1f)
        {
            if (Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, rawAxis.y), .2f,
                WhatStopsMovement))
            {
                //DO NOTHING
            }
            else if (Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, rawAxis.y),
                .2f, WhatCanBePushed))
            {
                GameObject pushedObject = Physics2D.OverlapCircle(
                        movePoint.position + new Vector3(0f, rawAxis.y), .2f,
                        WhatCanBePushed)
                    .gameObject;

                PushBlock(pushedObject, Vector2.down);
            }
            else
            {
                movePoint.position += new Vector3(0f, rawAxis.y);
                // if (undoManager.addMove(UndoManager.MovementType.DOWN))
                // {
                //     movePoint.position += new Vector3(0f, rawAxis.y);
                // }
                // else
                // {
                //     messengerManager.MessageTooManyMoves();
                // }
            }
        }
    }

    private void PushBlock(GameObject blockToPush, Vector2 direction)
    {
        /*if (blockToPush.GetComponent<MovingBlock>().MoveBlock(direction, moveSpeed))
        {
            if (direction == Vector2.down)
            {
                if (undoManager.addMove(UndoManager.MovementType.PUSHDOWN))
                {
                    movePoint.position += new Vector3(direction.x, direction.y);
                }
                else
                {
                    messengerManager.MessageTooManyMoves();
                }
            }
            if (direction == Vector2.up)
            {
                if (undoManager.addMove(UndoManager.MovementType.PUSHUP))
                {
                    movePoint.position += new Vector3(direction.x, direction.y);
                }
                else
                {
                    messengerManager.MessageTooManyMoves();
                }
            }
            if (direction == Vector2.left)
            {
                if (undoManager.addMove(UndoManager.MovementType.PUSHLEFT))
                {
                    movePoint.position += new Vector3(direction.x, direction.y);
                }
                else
                {
                    messengerManager.MessageTooManyMoves();
                }
            }
            if (direction == Vector2.right)
            {
                if (undoManager.addMove(UndoManager.MovementType.PUSHRIGHT))
                {
                    movePoint.position += new Vector3(direction.x, direction.y);
                }
                else
                {
                    messengerManager.MessageTooManyMoves();
                }
            }
        }*/
    }

    public void SetCanMove(bool canMoveIn)
    {
        canMove = canMoveIn;
    }

    public void BTN_Undo(InputAction.CallbackContext ctx)
    {
        if (canMove)
        {
            //UndoMovement();
        }
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

    public enum MoveDirections
    {
        Right,
        Left,
        Up,
        Down,
        Pause
    }
}