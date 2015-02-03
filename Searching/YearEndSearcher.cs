using System;
using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
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

        public IQueryable<Journal> FindJournalsWithin(SearchWindow<YearEndParameters> searchWindow)
        {
            var periodJournals = repository.GetJournalsApplyingTo(searchWindow.Period);

            var startOfSearchPeriod = GetCreationStartDate(searchWindow);
            return periodJournals.Where(x => x.Created >= startOfSearchPeriod);            
        }

        private static DateTimeOffset GetCreationStartDate(SearchWindow<YearEndParameters> searchWindow)
        {
            var periodEndDate = searchWindow.Period.To;
            return periodEndDate.Subtract(TimeSpan.FromDays(searchWindow.Parameters.DaysBeforeYearEnd));
        }
    }
}