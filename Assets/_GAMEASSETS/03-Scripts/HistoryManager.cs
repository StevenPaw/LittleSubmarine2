using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LittleSubmarine2
{
    public class HistoryManager : MonoBehaviour
    {
        [SerializeField] private List<MoveTypes> moves = new List<MoveTypes>();
        
        public void addMove(MoveTypes type)
        {
            moves.Add(type);
        }

        public MoveTypes getUndoMove()
        {
            if (moves.Any())
            {
                MoveTypes output = moves[moves.Count - 1];
                moves.RemoveAt(moves.Count - 1);
                return output;
            }
            else
            {
                return MoveTypes.EMPTY; //Returns empty undo if on Start Position
            }
        }
    }
}