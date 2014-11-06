using System.Collections.Generic;
using Model.Accounting;

namespace Model.Responses
{
    public class SearchResponse
    {
        public SearchResponse(IList<Journal> journals, int totalResults)
        {
            TotalResults = totalResults;
            Journals = journals;
        }

        public IList<Journal> Journals { get; private set; } 
        public int TotalResults { get; private set; } 
    }
}
