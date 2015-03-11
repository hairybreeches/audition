using System;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;

namespace Searching.SearchWindows
{
    public class EndingParameters : ISearchParameters
    {
        public int MinimumZeroesToBeConsideredUnusual { get; private set; }

        public EndingParameters(int minimumZeroesToBeConsideredUnusual)
        {
            MinimumZeroesToBeConsideredUnusual = minimumZeroesToBeConsideredUnusual;
        }

        public int Magnitude()
        {
            return (int) Math.Pow(10, MinimumZeroesToBeConsideredUnusual);
        }

        public override string ToString()
        {
            return Description;
        }

        public string Description
        {
            get { return String.Format("ending in at least {0} zeroes", MinimumZeroesToBeConsideredUnusual); }
        }

        public IQueryable<Transaction> ApplyFilter(Searcher searcher, IQueryable<Transaction> transactions)
        {
            return searcher.FindTransactionsWithin(this, transactions);
        }
    }
}