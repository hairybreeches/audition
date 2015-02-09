using System;
using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;
using Searching.SearchWindows;

namespace Searching
{
    public class YearEndSearcher : IJournalSearcher<YearEndParameters>
    {
        private readonly IJournalRepository repository;

        public YearEndSearcher(IJournalRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<Journal> FindJournalsWithin(YearEndParameters parameters, DateRange dateRange)
        {
            var periodJournals = repository.GetJournalsApplyingTo(dateRange);

            var startOfSearchPeriod = GetCreationStartDate(parameters);
            return periodJournals.Where(x => x.Created >= startOfSearchPeriod);            
        }

        private static DateTimeOffset GetCreationStartDate(YearEndParameters parameters)
        {            
            return parameters.YearEnd.Subtract(TimeSpan.FromDays(parameters.DaysBeforeYearEnd));
        }
    }
}