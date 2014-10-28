using System;

namespace Model.SearchWindows
{
    public class YearEndParameters
    {
        public YearEndParameters(int daysBeforeYearEnd)
        {
            DaysBeforeYearEnd = daysBeforeYearEnd;
        }

        public int DaysBeforeYearEnd { get; private set; }

        public override string ToString()
        {
            return String.Format("posted after the year end or within {0} days before", DaysBeforeYearEnd);
        }
    }
}