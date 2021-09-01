using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LittleSubmarine2
{
    public class MovingBlock : MonoBehaviour, IPushable
{
    [SerializeField] private Transform movePoint;
    [SerializeField] private LayerMask WhatStopsMovement;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private bool canMoveOtherBlocks = false;
    [SerializeField] private LayerMask WhatCanBeMoved;
    [SerializeField] private PushableTypes blockType;
    
    private HistoryManager history;

    private void Start()
    {
        history = GetComponent<HistoryManager>();
        movePoint.parent = null;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
    }

    public void UndoPush()
    {
        MoveTypes undoMove = history.getUndoMove();
        if (undoMove == MoveTypes.MOVEUP)
        {
            MoveBlockBack(Vector2.down, moveSpeed);
        }
        if (undoMove == MoveTypes.MOVEDOWN)
        {
            MoveBlockBack(Vector2.up, moveSpeed);
        }
        if (undoMove == MoveTypes.MOVELEFT)
        {
            MoveBlockBack(Vector2.right, moveSpeed);
        }
        if (undoMove == MoveTypes.MOVERIGHT)
        {
            MoveBlockBack(Vector2.left, moveSpeed);
        }
    }
    
    public bool Push(Vector2 moveDirection, float moveSpeedIn)
    {
        moveSpeed = moveSpeedIn;

        //RIGHT MOVEMENT
        if (moveDirection == Vector2.right)
        {
            if (Physics2D.OverlapCircle(movePoint.position + new Vector3(.5f, 0f), .2f, WhatStopsMovement))
            {
                //DO NOTHING
            }
            else if (Physics2D.OverlapCircle(movePoint.position + new Vector3(.5f, 0f), .2f, WhatCanBeMoved))
            {
                if (canMoveOtherBlocks)
                {
                    //TODO: Push other Block
                    history.addMove(MoveTypes.PUSHRIGHT);
                    movePoint.position += new Vector3(1f, 0f);
                    return true;
                }
            }
            else
            {
                history.addMove(MoveTypes.MOVERIGHT);
                movePoint.position += new Vector3(1f, 0f);
                return true;
            }
        }
        //LEFT MOVEMENT
        else if (moveDirection == Vector2.left)
        {
            if (Physics2D.OverlapCircle(movePoint.position + new Vector3(-.5f, 0f), .2f, WhatStopsMovement))
            {
                //DO NOTHING
            }
            else if (Physics2D.OverlapCircle(movePoint.position + new Vector3(-.5f, 0f), .2f, WhatCanBeMoved))
            {
                if (canMoveOtherBlocks)
                {
                    //TODO: Push other Block
                    history.addMove(MoveTypes.PUSHLEFT);
                    movePoint.position += new Vector3(-1f, 0f);
                    return true;
                }
            }
            else
            {
                history.addMove(MoveTypes.MOVELEFT);
                    movePoint.position += new Vector3(-1f, 0f);
                    return true;
            }
        }

        //UP MOVEMENT
        else if (moveDirection == Vector2.up)
        {
            if (Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, .5f), .2f, WhatStopsMovement))
            {
                
            }
            else if (Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, .5f), .2f, WhatCanBeMoved))
            {
                if (canMoveOtherBlocks)
                {
                    //TODO: Push other Block
                    history.addMove(MoveTypes.PUSHUP);
                    movePoint.position += new Vector3(0f, 1f);
                    return true;
                }
            }
            else
            {
                history.addMove(MoveTypes.MOVEUP);
                movePoint.position += new Vector3(0f, 1f);
                return true;
            }
        }
        //DOWN MOVEMENT
        else if (moveDirection == Vector2.down)
        {
            if (Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, -.5f), .2f, WhatStopsMovement))
            {
                //DO NOTHING
            }
            else if (Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, -.5f), .2f, WhatCanBeMoved))
            {
                if (canMoveOtherBlocks)
                {
                    history.addMove(MoveTypes.PUSHDOWN);
                    movePoint.position += new Vector3(0f, -1f);
                    return true;
                }
            }
            else
            {
                history.addMove(MoveTypes.MOVEDOWN);
                movePoint.position += new Vector3(0f, -1f);
                return true;
            }
        }

        return false;
    }
    
    private bool MoveBlockBack(Vector2 moveDirection, float moveSpeedIn)
    {
        moveSpeed = moveSpeedIn;
        
        //RIGHT MOVEMENT
        if (moveDirection == Vector2.right)
        {
            if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(1f, 0f), .2f, WhatStopsMovement))
            {
                movePoint.position += new Vector3(1f, 0f);
                return true;
            }
        }
        //LEFT MOVEMENT
        else if (moveDirection == Vector2.left)
        {
            if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(-1f, 0f), .2f, WhatStopsMovement))
            {
                movePoint.position += new Vector3(-1f, 0f);
                return true;
            }
        }

        //UP MOVEMENT
        else if (moveDirection == Vector2.up)
        {
            if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, 1f), .2f, WhatStopsMovement))
            {
                movePoint.position += new Vector3(0f, 1f);
                return true;
            }
        }
        //DOWN MOVEMENT
        else if (moveDirection == Vector2.down)
        {
            if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, -1f), .2f, WhatStopsMovement))
            {
                movePoint.position += new Vector3(0f, -1f);
                return true;
            }
        }

        return false;
    }

    public PushableTypes GetPushableType()
    {
        return blockType;
    }
}
}