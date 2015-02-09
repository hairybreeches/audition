using System;
using System.Linq;
using Model.Accounting;
using Model.Time;

namespace Searching.SearchWindows
{
    public class YearEndParameters : ISearchParameters
    {
        public YearEndParameters(int daysBeforeYearEnd)
        {
            DaysBeforeYearEnd = daysBeforeYearEnd;
        }

        public int DaysBeforeYearEnd { get; private set; }


        public Func<DateRange, IQueryable<Journal>> GetSearchMethod(JournalSearcher searcher)
        {
            return dateRange => searcher.FindJournalsWithin(this, dateRange);
        }

        public override string ToString()
        {
            return String.Format("posted after the year end or within {0} days before", DaysBeforeYearEnd);
        }
    }
}