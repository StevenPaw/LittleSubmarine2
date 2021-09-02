namespace LittleSubmarine2
{
    public interface IActivator
    {
        public bool GetActivated();
        public void Activate();
        public void DeActivate();
        public void AddActivatable(IActivatable activatableIn);
    }
}