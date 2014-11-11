using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.SearchWindows;
using Persistence;

namespace Searching
{
    public class YearEndSearcher : IJournalSearcher<YearEndParameters>
    {
        private readonly JournalRepository repository;

        public YearEndSearcher(JournalRepository repository)
        {
            this.repository = repository;
        }               

        public IQueryable<Journal> FindJournalsWithin(SearchWindow<YearEndParameters> searchWindow)
        {
            var periodJournals = repository.GetJournalsApplyingTo(searchWindow.Period);

            var startOfSearchPeriod = searchWindow.CreationStartDate();
            return periodJournals.Where(x => x.Created >= startOfSearchPeriod);            
        }
    }
}