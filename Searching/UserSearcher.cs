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
        public IQueryable<Journal> FindJournalsWithin(UserParameters parameters, DateRange dateRange, IJournalRepository repository)
        {
            var lookup = new HashSet<string>(parameters.Usernames, StringComparer.Create(CultureInfo.CurrentCulture, true));
            return repository.GetJournalsApplyingTo(dateRange).Where(x=> !lookup.Contains(x.Username));
        }
    }
}
