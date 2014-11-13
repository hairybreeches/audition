using System.Collections.Generic;
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
    }
}
