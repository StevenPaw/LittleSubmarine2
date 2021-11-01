using UnityEngine;

namespace LittleSubmarine2
{
    public class Move : Command
    {
        private IMovable movable;
        private Vector2 direction;
        
        public Move(IMovable movable, Vector2 direction)
        {
            this.movable = movable;
            this.direction = direction;
        }
        
        public override bool Execute()
        {
            bool canLeaveTile = true;

            if (movable.Anim != null)
            {
                movable.Anim.SetFloat("horizontal", direction.x);
                movable.Anim.SetFloat("vertical", direction.y);
            }

            if (Physics2D.OverlapCircle((Vector2) movable.MovePoint.position, .2f, LayerTypes.SPECIALTILES))
            {
                canLeaveTile = TileHelper.CanLeaveSpecialTile(movable.SelfCollider, direction, LayerTypes.SPECIALTILES);
            }
            
            if (canLeaveTile)
            {
                movable.MovePoint.position += new Vector3(direction.x, direction.y);
                movable.History.addMove(this);
                return true;
            }

            return false;
        }

        public override bool Undo()
        {
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