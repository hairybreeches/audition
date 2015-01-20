using System;
using System.Collections.Generic;
using System.Linq;
using Model.Accounting;

namespace Model.Responses
{
    public class SearchResponse
    {
        public SearchResponse(IList<Journal> journals, string totalResults, bool isPreviousPage, bool isNextPage, int firstResult)
        {
            FirstResult = firstResult;
            IsNextPage = isNextPage;
            IsPreviousPage = isPreviousPage;
            TotalResults = totalResults;
            Journals = journals;
        }

        public IList<Journal> Journals { get; private set; } 
        public string TotalResults { get; private set; }
        public bool IsPreviousPage { get; private set; }
        public bool IsNextPage { get; private set; }        
        public int FirstResult { get; private set; }

        public override string ToString()
        {
            return
                String.Format(
                    "Total results: {0}, First Result of this page: {1}, previous page: {2}, next page: {3}, journals: {4}",
                    TotalResults, FirstResult, IsPreviousPage, IsNextPage, String.Join(", ", Journals));
        }

        protected bool Equals(SearchResponse other)
        {
            return Journals.SequenceEqual(other.Journals) 
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
                var hashCode = (Journals != null ? Journals.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (TotalResults != null ? TotalResults.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ IsPreviousPage.GetHashCode();
                hashCode = (hashCode*397) ^ IsNextPage.GetHashCode();
                hashCode = (hashCode*397) ^ FirstResult;
                return hashCode;
            }
        }
    }
}
