using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LittleSubmarine2
{
    public class MovingBlock : MonoBehaviour, IPushable
{
    [SerializeField] private Transform movePoint;
    [SerializeField] private LayerMask WhatStopsMovement;
    [SerializeField] private float moveSpeed = 5f;
    public LayerMask specialTiles;
    [SerializeField] private PushableTypes blockType;
    
    private HistoryManager history;
    private Collider2D selfCollider;

    private void Start()
    {
        history = GetComponent<HistoryManager>();
        selfCollider = GetComponent<Collider2D>();
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
    
    public bool Push(Vector2 direction, float moveSpeedIn)
    {
        moveSpeed = moveSpeedIn;

        MoveTypes moveType = MoveTypes.EMPTY;
        
        if (direction == Vector2.down)
        {
            moveType = MoveTypes.MOVEDOWN;
        }
        if (direction == Vector2.up)
        {
            moveType = MoveTypes.MOVEUP;
        }
        if (direction == Vector2.left)
        {
            moveType = MoveTypes.MOVELEFT;
        }
        if (direction == Vector2.right)
        {
            moveType = MoveTypes.MOVERIGHT;
        }

        if (Physics2D.OverlapCircle((Vector2) movePoint.position, .2f, specialTiles))
        {
            if (!TileHelper.CanLeaveSpecialTile(selfCollider, direction, specialTiles))
            {
                return false;
            }
        }
        
        if (Physics2D.OverlapCircle((Vector2)movePoint.position + direction, .2f, WhatStopsMovement))
        {
            //DO NOTHING
        }
        else if (Physics2D.OverlapCircle((Vector2)movePoint.position + direction, .2f, specialTiles))
        {
            SpecialTile tile = Physics2D.OverlapCircle((Vector2)movePoint.position + direction, .2f, specialTiles).GetComponent<SpecialTile>();
            if (tile.GetTileType() == SpecialTileTypes.PUSHABLE)
            {
                return false;
            }
            else if (TileHelper.CanUseSpecialTile(tile, direction, moveType,true, moveSpeed, history))
            {
                //TODO: Push other Block
                movePoint.position += new Vector3(direction.x, direction.y);
                return true;
            }
        }
        else
        {
            history.addMove(moveType);
            movePoint.position += new Vector3(direction.x, direction.y);
            return true;
        }

        return false;
    }
    
    /*private bool CanUseSpecialTile(SpecialTile tileIn, Vector2 direction, MoveTypes moveType, bool saveHistory)
    {
        Debug.Log("Test Special Tile: " + tileIn.name);
        switch (tileIn.GetTileType())
        {
            default:
                history.addMove(moveType);
                return true;
            case SpecialTileTypes.ONEWAY:
                Oneway ow = tileIn.GetComponent<Oneway>();
                history.addMove(moveType);
                return direction == ow.GetDirection();
            case SpecialTileTypes.PUSHABLE:
                if (canMoveOtherBlocks)
                {
                    history.addMove(moveType);
                    return true;
                }
                return false;
        }
    }
    
    private bool CanLeaveSpecialTile(SpecialTile tileIn, Vector2 direction, MoveTypes moveType)
    {
        Debug.Log("Test Special Tile: " + tileIn.name);
        switch (tileIn.GetTileType())
        {
            default:
                return true;
            case SpecialTileTypes.ONEWAY:
                Oneway ow = tileIn.GetComponent<Oneway>();
                return direction == ow.GetDirection();
        }
    }*/
    
    private bool MoveBlockBack(Vector2 direction, float moveSpeedIn)
    {
        moveSpeed = moveSpeedIn;

        if (!Physics2D.OverlapCircle(movePoint.position + new Vector3(direction.x, direction.y), .2f, WhatStopsMovement))
        {
            movePoint.position += new Vector3(direction.x, direction.y);
            return true;
        }

        return false;
    }

    public PushableTypes GetPushableType()
    {
        return blockType;
    }
}
}