using System.Collections.Generic;
using Model.Accounting;
using Model.SearchWindows;

namespace Model.Searching
{
    public class UserSearcher : IJournalSearcher<UserParameters>
    {
        private readonly JournalRepository repository;

        public UserSearcher(JournalRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<UserParameters> searchWindow)
        {
            return repository.GetJournalsApplyingTo(searchWindow.Period);
        }
    }
}
