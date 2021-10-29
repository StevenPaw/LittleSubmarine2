using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LittleSubmarine2
{
    public class HistoryManager : MonoBehaviour
    {
        private Stack<Command> moves = new Stack<Command>();
        
        public void addMove(Command command)
        {
            moves.Push(command);
        }

        public Command getUndoMove()
        {
            if (moves.Any())
            {
                Command output = moves.Pop();
                return output;
            }
            else
            {
                return null; //Returns empty undo if stack is empty
            }
        }
    }
}