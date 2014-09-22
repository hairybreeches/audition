
using Model.Time;

namespace Model.SearchWindows
{    
    public class HoursSearchWindow
    {
        public HoursSearchWindow(WorkingHours parameters, DateRange period)
        {
            Period = period;
            Parameters = parameters;
        }

        public WorkingHours Parameters { get; private set; }
        public DateRange Period { get; private set; }

        public override string ToString()
        {
            return string.Format("Outside {0}, in the period {1}", Parameters, Period);
        }

        protected bool Equals(HoursSearchWindow other)
        {
            return Equals(Parameters, other.Parameters) && Equals(Period, other.Period);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HoursSearchWindow) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Parameters != null ? Parameters.GetHashCode() : 0)*397) ^ (Period != null ? Period.GetHashCode() : 0);
            }
        }
    }
}