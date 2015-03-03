using System.Collections.Generic;
using System.Linq;
using Model.Accounting;

namespace Searching
{
    public class NominalCodeLookup
    {
        private readonly IDictionary<string, int> lookup = new Dictionary<string, int>();

        public NominalCodeLookup(IEnumerable<Transaction> transactions)
        {
            Add(transactions);
        }



        private void Add(IEnumerable<Transaction> transactions)
        {
            foreach (var transaction in transactions)
            {
                Add(transaction);
            }
        }

        private void Add(Transaction transaction)
        {
            foreach (var line in transaction.Lines)
            {
                Add(line.NominalCode);
            }
        }

        private void Add(string nominalCode)
        {
            if (!lookup.ContainsKey(nominalCode))
            {
                lookup[nominalCode] = 0;
            }

            lookup[nominalCode] ++;
        }

        public ISet<string> UnusualNominalCodes(int minimumEntriesToBeConsideredNormal)
        {
            return new HashSet<string>(lookup.Where(x => x.Value < minimumEntriesToBeConsideredNormal).Select(x=>x.Key));
        }
    }
}