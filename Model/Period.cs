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
    }
}