using System.Collections.Generic;
using System.Linq;
using Model.Accounting;

namespace Searching
{
    public class AccountsLookup
    {
        private readonly IDictionary<string, int> lookup = new Dictionary<string, int>();

        public AccountsLookup(IEnumerable<Transaction> journals)
        {
            Add(journals);
        }



        private void Add(IEnumerable<Transaction> journals)
        {
            foreach (var journal in journals)
            {
                Add(journal);
            }
        }

        private void Add(Transaction transaction)
        {
            foreach (var line in transaction.Lines)
            {
                Add(line.AccountCode);
            }
        }

        private void Add(string accountCode)
        {
            if (!lookup.ContainsKey(accountCode))
            {
                lookup[accountCode] = 0;
            }

            lookup[accountCode] ++;
        }

        public ISet<string> UnusualAccountCodes(int minimumEntriesToBeConsideredNormal)
        {
            return new HashSet<string>(lookup.Where(x => x.Value < minimumEntriesToBeConsideredNormal).Select(x=>x.Key));
        }
    }
}