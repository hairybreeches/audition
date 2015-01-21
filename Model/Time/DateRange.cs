using System;

namespace Model.Time
{
    public class DateRange
    {
        public DateRange(DateTime @from, DateTime to)
        {
            To = to.Date;
            From = @from.Date;
        }

        public DateTime From { get; private set; }
        public DateTime To { get; private set; }

        public override string ToString()
        {
            return String.Format("{0} to {1}", From.ToShortDateString(), To.ToShortDateString());
        }

        protected bool Equals(DateRange other)
        {
            return From.Equals(other.From) && To.Equals(other.To);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DateRange) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (From.GetHashCode()*397) ^ To.GetHashCode();
            }
        }

        public bool Contains(DateTime journalDateTime)
        {
            var journalDate = journalDateTime.Date;
            return journalDate >= From
                   && journalDate <= To;
        }
    }
}