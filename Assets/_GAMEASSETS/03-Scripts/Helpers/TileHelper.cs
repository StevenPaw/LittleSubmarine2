using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LittleSubmarine2
{
    public static class TileHelper
    {
        public static bool CanUseSpecialTile(SpecialTile tileIn, Vector2 direction, MoveTypes moveType, bool saveHistory, float moveSpeed, HistoryManager history)
        {
            switch (tileIn.GetTileType())
            {
                default:
                    history.addMove(moveType);
                    return true;
                case SpecialTileTypes.ONEWAY:
                    Oneway ow = tileIn.GetComponent<Oneway>();
                    if (direction == ow.GetDirection())
                    {
                        history.addMove(moveType);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case SpecialTileTypes.PUSHABLE:
                    GameObject pushedObject = tileIn.gameObject;
                    return PushBlock(pushedObject, direction, moveSpeed, history);
            }
        }
        
        private static bool CanLeaveSpecialTile(SpecialTile tileIn, Vector2 direction)
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
        }

        public static bool CanLeaveSpecialTile(Collider2D selfCollider, Vector2 direction, LayerMask SpecialTiles)
        {
            List<Collider2D> collidersAtPosition = Physics2D.OverlapCircleAll(selfCollider.transform.position, .2f, SpecialTiles).ToList();
            if(collidersAtPosition.Contains(selfCollider)) {
                collidersAtPosition.Remove(selfCollider);
            }
            
            foreach(Collider2D col in collidersAtPosition)
            {
                SpecialTile tile = col.GetComponent<SpecialTile>();
                if(!CanLeaveSpecialTile(tile, direction))
                {
                    return false;
                }
            }

            return true;
        }
        
        public static bool PushBlock(GameObject blockToPush, Vector2 direction, float moveSpeed, HistoryManager history)
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
    }
    
}