using System;
using System.Linq;
using Model.Accounting;

namespace Searching.SearchWindows
{
    public class DuplicatePaymentsParameters : ISearchParameters
    {
        public DuplicatePaymentsParameters(int maximumDaysBetweenTransactions)
        {
            MaximumDaysBetweenTransactions = maximumDaysBetweenTransactions;
        }

        public int MaximumDaysBetweenTransactions { get; private set; }

        public IQueryable<Transaction> ApplyFilter(Searcher searcher, IQueryable<Transaction> transactions)
        {
            return searcher.FindTransactionsWithin(this, transactions);
        }

        public string Description
        {
            get { return String.Format("with the same type, amount and nominal code within {0} days", MaximumDaysBetweenTransactions); }
        }

        public override string ToString()
        {
            return Description;
        }
    }
}