using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;
using Searching.SearchWindows;

namespace Searching
{
    public class UserSearcher : IJournalSearcher<UserParameters>
    {
        private readonly IJournalRepository repository;

        public UserSearcher(IJournalRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<Journal> FindJournalsWithin(UserParameters parameters, DateRange dateRange)
        {
            var lookup = new HashSet<string>(parameters.Usernames, StringComparer.Create(CultureInfo.CurrentCulture, true));
            return repository.GetJournalsApplyingTo(dateRange).Where(x=> !lookup.Contains(x.Username));
        }
    }
}
