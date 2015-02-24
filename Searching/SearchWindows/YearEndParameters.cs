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
        //todo: this is duplicate information with its containing SearchWindow and they could get out of sync
        public DateTime YearEnd { get; private set; }


        public IQueryable<Transaction> ApplyFilter(Searcher searcher, IQueryable<Transaction> transactions)
        {
            return searcher.FindTransactionsWithin(this, transactions);
        }

        public override string ToString()
        {
            return String.Format("posted after the year end or within {0} days before", DaysBeforeYearEnd);
        }
    }
}