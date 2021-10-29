using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LittleSubmarine2
{
    public static class TileHelper
    {
        public static bool CanUseSpecialTile(SpecialTile tileIn, Vector2 direction, bool saveHistory, IMovable movable)
        {
            switch (tileIn.GetTileType())
            {
                default:
                    Command simplemove = new Move(movable, direction);
                    if (simplemove.Execute())
                    {
                        return true;
                    }
                    return false;
                
                case SpecialTileTypes.ONEWAY:
                    Oneway ow = tileIn.GetComponent<Oneway>();
                    if (direction == ow.GetDirection())
                    {
                        Command move = new Move(movable, direction);
                        if (move.Execute())
                        {
                            return true;
                        }
                    }
                    return false;
                
                case SpecialTileTypes.PUSHABLE:
                    IPushable pushedObject = tileIn.gameObject.GetComponent<IPushable>();
                    if (pushedObject.Push(direction))
                    {
                        Command pushMove = new Push(movable, pushedObject, direction);
                        if (pushMove.Execute())
                        {
                            return true;
                        }
                    }
                    return false;
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
    }
    
}