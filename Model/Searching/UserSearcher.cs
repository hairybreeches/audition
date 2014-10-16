using System.Collections.Generic;
using System.Linq;
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
            var lookup = new HashSet<string>(searchWindow.Parameters.Usernames);
            return repository.GetJournalsApplyingTo(searchWindow.Period).Where(x=> !lookup.Contains(x.Username));
        }
    }
}
