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

        protected bool Equals(UnusualAccountsParameters other)
        {
            return Quantity == other.Quantity;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UnusualAccountsParameters) obj);
        }

        public override int GetHashCode()
        {
            return Quantity;
        }
    }
}