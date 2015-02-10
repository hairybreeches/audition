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


        public IQueryable<Journal> ApplyFilter(JournalSearcher searcher, IQueryable<Journal> journals)
        {
            return searcher.FindJournalsWithin(this, journals);
        }

        public override string ToString()
        {
            return String.Format("posted after the year end or within {0} days before", DaysBeforeYearEnd);
        }
    }
}