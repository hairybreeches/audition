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
    public class UserSearcher : ISearcher<UserParameters>
    {
        public IQueryable<Transaction> FindTransactionsWithin(UserParameters parameters, IQueryable<Transaction> transactions)
        {
            var lookup = new HashSet<string>(parameters.Usernames, StringComparer.Create(CultureInfo.CurrentCulture, true));
            return transactions.Where(x=> !lookup.Contains(x.Username));
        }
    }
}
