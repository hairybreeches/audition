﻿using System;
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
        private readonly JournalRepository repository;

        public UserSearcher(JournalRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<UserParameters> searchWindow)
        {
            var lookup = new HashSet<string>(searchWindow.Parameters.Usernames, StringComparer.Create(CultureInfo.CurrentCulture, true));
            return repository.GetJournalsApplyingTo(searchWindow.Period).Where(x=> !lookup.Contains(x.Username));
        }
    }
}