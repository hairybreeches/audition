using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
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

        [HttpGet]
        [Route("api/search")]
        public IEnumerable<Journal> Search([ModelBinder(typeof(JsonModelBinder))] SearchWindow searchWindow)
        {
            return Enumerable.Empty<Journal>();            
        }
    }
}
