using System;

namespace Model
{
    public class Period
    {
        public Period(DateTime fromDate, DateTime to)
        {
            To = to;
            From = fromDate;
        }

        public DateTime From { get; private set; }
        public DateTime To { get; private set; }

        public override string ToString()
        {
            return String.Format("{0} to {1}", From, To);
        }
    }
}