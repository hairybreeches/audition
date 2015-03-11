using System;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;

namespace Searching.SearchWindows
{
    public class UnusualNominalCodesParameters : ISearchParameters
    {
        public UnusualNominalCodesParameters(int minimumEntriesToBeConsideredNormal)
        {
            MinimumEntriesToBeConsideredNormal = minimumEntriesToBeConsideredNormal;
        }
        
        public int MinimumEntriesToBeConsideredNormal { get; private set; }


        public IQueryable<Transaction> ApplyFilter(Searcher searcher, IQueryable<Transaction> transactions)
        {
            return searcher.FindTransactionsWithin(this, transactions);
        }
        
        public override string ToString()
        {
            return Description;
        }

        public string Description
        {
            get
            {
                return string.Format("posted to nominal codes with fewer than {0} entries", MinimumEntriesToBeConsideredNormal);
            }
        }

        protected bool Equals(UnusualNominalCodesParameters other)
        {
            return MinimumEntriesToBeConsideredNormal == other.MinimumEntriesToBeConsideredNormal;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((UnusualNominalCodesParameters) obj);
        }

        public override int GetHashCode()
        {
            return MinimumEntriesToBeConsideredNormal;
        }
    }
}