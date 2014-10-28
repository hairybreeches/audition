using System.Collections.Generic;
using System.Linq;
using Model.Accounting;

namespace Audition.Responses
{
    public class SearchResponse
    {
        public SearchResponse(IEnumerable<Journal> journals, int totalResults)
        {
            TotalResults = totalResults;
            Journals = journals.ToArray();
        }

        public Journal[] Journals { get; private set; } 
        public int TotalResults { get; private set; } 
    }
}
