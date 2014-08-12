using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Model;

namespace Audition.Controllers
{
    public class SearchController : ApiController
    {
        private readonly IJournalSearcher searcher;

        public SearchController(IJournalSearcher searcher)
        {
            this.searcher = searcher;
        }

        public IEnumerable<Journal> Search(SearchWindow searchWindow)
        {
            return Enumerable.Empty<Journal>();            
        }
    }
}
