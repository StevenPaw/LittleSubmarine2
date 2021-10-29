using UnityEngine;

namespace LittleSubmarine2
{
    public class Push : Command
    {
        private IMovable movable;
        private IPushable pushable;
        private Vector2 direction;
        
        public Push(IMovable movable, IPushable pushable, Vector2 direction)
        {
            this.movable = movable;
            this.pushable = pushable;
            this.direction = direction;
        }
        
        public override bool Execute()
        {
            bool canLeaveTile = true;
            
            movable.Anim.SetFloat("horizontal", direction.x);
            movable.Anim.SetFloat("vertical", direction.y);
                
            if (Physics2D.OverlapCircle((Vector2) movable.MovePoint.position, .2f, LayerTypes.SPECIALTILES))
            {
                canLeaveTile = TileHelper.CanLeaveSpecialTile(movable.SelfCollider, direction, LayerTypes.SPECIALTILES);
            }
            
            if (canLeaveTile)
            {
                Debug.Log("Can Leave Tile");
                movable.MovePoint.position += new Vector3(direction.x, direction.y);
                movable.History.addMove(this);
                return true;
            }
            else
            {
                Debug.Log("Cant Leave Tile!");
            }

            return false;
        }

        public override bool Undo()
        {
            pushable.UndoPush();
            
            if (movable.Anim != null)
            {
                movable.Anim.SetFloat("horizontal", direction.x);
                movable.Anim.SetFloat("vertical", direction.y);
            }

            movable.MovePoint.position -= new Vector3(direction.x, direction.y);

            return true;
        }
    }
}