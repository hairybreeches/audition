using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Model.Accounting;
using Model.SearchWindows;
using Persistence;

namespace Searching
{
    public class UserSearcher : IJournalSearcher<UserParameters>
    {
        private readonly IJournalRepository repository;

        public UserSearcher(IJournalRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<Journal> FindJournalsWithin(SearchWindow<UserParameters> searchWindow)
        {
            var lookup = new HashSet<string>(searchWindow.Parameters.Usernames, StringComparer.Create(CultureInfo.CurrentCulture, true));
            return repository.GetJournalsApplyingTo(searchWindow.Period).Where(x=> !lookup.Contains(x.Username));
        }
    }
}
