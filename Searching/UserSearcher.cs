﻿using System;
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
        public IQueryable<Transaction> FindJournalsWithin(UserParameters parameters, IQueryable<Transaction> journals)
        {
            var lookup = new HashSet<string>(parameters.Usernames, StringComparer.Create(CultureInfo.CurrentCulture, true));
            return journals.Where(x=> !lookup.Contains(x.Username));
        }
    }
}
