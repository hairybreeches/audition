
namespace Model
{    
    public class SearchWindow
    {
        public SearchWindow(TimeFrame outside, Period period)
        {
            Period = period;
            Outside = outside;
        }

        public TimeFrame Outside { get; private set; }
        public Period Period { get; private set; }

        public override string ToString()
        {
            return string.Format("Outside {0}, in the period {1}", Outside, Period);
        }

        protected bool Equals(SearchWindow other)
        {
            return Equals(Outside, other.Outside) && Equals(Period, other.Period);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SearchWindow) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Outside != null ? Outside.GetHashCode() : 0)*397) ^ (Period != null ? Period.GetHashCode() : 0);
            }
        }
    }
}