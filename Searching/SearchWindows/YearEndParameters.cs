using System;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;

namespace Searching.SearchWindows
{
    public class YearEndParameters : ISearchParameters
    {
        public YearEndParameters(int daysBeforeYearEnd, DateTime yearEnd)
        {
            DaysBeforeYearEnd = daysBeforeYearEnd;
            YearEnd = yearEnd;
        }

        public int DaysBeforeYearEnd { get; private set; }
        public DateTime YearEnd { get; private set; }


        public Func<DateRange, IQueryable<Journal>> GetSearchMethod(JournalSearcher searcher, IJournalRepository repository)
        {
            return dateRange => searcher.FindJournalsWithin(this, dateRange, repository);
        }

        public override string ToString()
        {
            return String.Format("posted after the year end or within {0} days before", DaysBeforeYearEnd);
        }
    }
}