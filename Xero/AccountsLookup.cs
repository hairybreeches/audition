﻿using System;
using System.Collections.Generic;
using System.Linq;
using XeroApi.Model;

namespace Xero
{
    public class AccountsLookup
    {
        private readonly IDictionary<string, IList<Journal>> lookup = new Dictionary<string, IList<Journal>>();

        public AccountsLookup(IEnumerable<Journal> journals)
        {
            Add(journals);
        }



        private void Add(IEnumerable<Journal> journals)
        {
            foreach (var journal in journals)
            {
                Add(journal);
            }
        }

        private void Add(Journal journal)
        {
            foreach (var line in journal.JournalLines)
            {
                Add(line.AccountCode, journal);
            }
        }

        private void Add(string accountCode, Journal journal)
        {
            IList<Journal> journalList;
            if (!lookup.TryGetValue(accountCode, out journalList))
            {
                journalList = lookup[accountCode] = new List<Journal>();
            }

            journalList.Add(journal);
        }

        public IEnumerable<Journal> JournalsMadeToUnusualAccountCodes(int quantity)
        {
            return lookup.Values.Where(x => x.Count < quantity).SelectMany(x => x).Distinct(new IdEqualityComparer());
        }
    }
}