
namespace Model
{
    public class SearchWindow
    {
        public SearchWindow(TimeFrame timeFrame, Period period)
        {
            Period = period;
            TimeFrame = timeFrame;
        }

        public TimeFrame TimeFrame { get; private set; }
        public Period Period { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}, in the period {1}", TimeFrame, Period);
        }

        protected bool Equals(SearchWindow other)
        {
            return Equals(TimeFrame, other.TimeFrame) && Equals(Period, other.Period);
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
                return ((TimeFrame != null ? TimeFrame.GetHashCode() : 0)*397) ^ (Period != null ? Period.GetHashCode() : 0);
            }
        }
    }
}