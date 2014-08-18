
namespace Model
{    
    public class SearchWindow
    {
        public SearchWindow(TimeFrame outside, DateRange dateRange)
        {
            DateRange = dateRange;
            Outside = outside;
        }

        public TimeFrame Outside { get; private set; }
        public DateRange DateRange { get; private set; }

        public override string ToString()
        {
            return string.Format("Outside {0}, in the period {1}", Outside, DateRange);
        }

        protected bool Equals(SearchWindow other)
        {
            return Equals(Outside, other.Outside) && Equals(DateRange, other.DateRange);
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
                return ((Outside != null ? Outside.GetHashCode() : 0)*397) ^ (DateRange != null ? DateRange.GetHashCode() : 0);
            }
        }
    }
}