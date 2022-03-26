namespace LittleSubmarine2.Events
{
    public class MoneyChangeEvent
    {
        private int amount;

        public int Amount => amount;

        public MoneyChangeEvent(int amount)
        {
            this.amount = amount;
        }
    }
}