using Model.Time;

namespace Model
{
    public class AccountsSearchWindow
    {
        public AccountsSearchWindow(int quantity, DateRange period)
        {
            Quantity = quantity;
            Period = period;
        }
        
        public int Quantity { get; private set; }
        public DateRange Period { get; private set; }

        public override string ToString()
        {
            return string.Format("Fewer than {0} entries, in the period {1}", Quantity, Period);
        }        
    }
}