using Model.Time;
using NodaTime;

namespace Model.SearchWindows
{
    public class UnusualAccountsParameters
    {
        public UnusualAccountsParameters(int quantity)
        {
            Quantity = quantity;
        }
        
        public int Quantity { get; private set; }
        
        public override string ToString()
        {
            return string.Format("Fewer than {0} entries", Quantity);
        }        
    }
}