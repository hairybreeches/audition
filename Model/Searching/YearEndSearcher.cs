using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Persistence;
using Model.SearchWindows;

namespace Model.Searching
{
    public class YearEndSearcher : IJournalSearcher<YearEndParameters>
    {
        private readonly InMemoryJournalRepository repository;

        public YearEndSearcher(InMemoryJournalRepository repository)
        {
            this.repository = repository;
        }               

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<YearEndParameters> searchWindow)
        {
            var periodJournals = repository.GetJournalsApplyingTo(searchWindow.Period).ToList();

            var startOfSearchPeriod = searchWindow.CreationStartDate();
            return periodJournals.Where(x => x.Created >= startOfSearchPeriod);            
        }
    }
}