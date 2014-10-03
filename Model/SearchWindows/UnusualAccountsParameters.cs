using Model.Time;
using NodaTime;

namespace Model.SearchWindows
{
    public class UnusualAccountsParameters
    {
        public UnusualAccountsParameters(int minimumEntriesToBeConsideredNormal)
        {
            MinimumEntriesToBeConsideredNormal = minimumEntriesToBeConsideredNormal;
        }
        
        public int MinimumEntriesToBeConsideredNormal { get; private set; }
        
        public override string ToString()
        {
            return string.Format("Fewer than {0} entries", MinimumEntriesToBeConsideredNormal);
        }

        protected bool Equals(UnusualAccountsParameters other)
        {
            return MinimumEntriesToBeConsideredNormal == other.MinimumEntriesToBeConsideredNormal;
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
            return MinimumEntriesToBeConsideredNormal;
        }
    }
}