namespace LittleSubmarine2
{
    public abstract class Command
    {
        //Should return if execution was successful
        public abstract bool Execute();
        
        //Should return if undoing was successful
        public abstract bool Undo();
    }
}