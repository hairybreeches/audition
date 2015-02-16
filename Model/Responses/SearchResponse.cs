using System;
using System.Collections.Generic;
using System.Linq;
using Model.Accounting;

namespace Model.Responses
{
    public class SearchResponse
    {
        public SearchResponse(IList<Transaction> transactions, string totalResults, bool isPreviousPage, bool isNextPage, int firstResult)
        {
            FirstResult = firstResult;
            IsNextPage = isNextPage;
            IsPreviousPage = isPreviousPage;
            TotalResults = totalResults;
            Transactions = transactions;
        }

        public IList<Transaction> Transactions { get; private set; } 
        public string TotalResults { get; private set; }
        public bool IsPreviousPage { get; private set; }
        public bool IsNextPage { get; private set; }        
        public int FirstResult { get; private set; }

        public override string ToString()
        {
            return
                String.Format(
                    "Total results: {0}, First Result of this page: {1}, previous page: {2}, next page: {3}, journals: {4}",
                    TotalResults, FirstResult, IsPreviousPage, IsNextPage, String.Join(", ", Transactions));
        }

        protected bool Equals(SearchResponse other)
        {
            return Transactions.SequenceEqual(other.Transactions) 
                && string.Equals(TotalResults, other.TotalResults) 
                && IsPreviousPage.Equals(other.IsPreviousPage) 
                && IsNextPage.Equals(other.IsNextPage) 
                && FirstResult == other.FirstResult;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SearchResponse) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Transactions != null ? Transactions.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (TotalResults != null ? TotalResults.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ IsPreviousPage.GetHashCode();
                hashCode = (hashCode*397) ^ IsNextPage.GetHashCode();
                hashCode = (hashCode*397) ^ FirstResult;
                return hashCode;
            }
        }
    }
}
