namespace LittleSubmarine2.Events
{
    public class ActivateButtonEvent
    {
        private bool isActivated;
        private string buttonKey;

        public bool IsActivated => isActivated;
        public string ButtonKey => buttonKey;
        
        public ActivateButtonEvent(bool isActivated, string buttonKey)
        {
            this.isActivated = isActivated;
            this.buttonKey = buttonKey;
        }
    }
}