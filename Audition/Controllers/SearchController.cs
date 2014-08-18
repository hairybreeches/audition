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
            return new List<Journal>
            {
                new Journal(Guid.NewGuid(), new DateTime(2012,3,4),new DateTime(2012,3,4), new List<JournalLine>
                {
                    new JournalLine("9012", "Expenses", JournalType.Cr, 23.4m),
                    new JournalLine("3001", "Cash", JournalType.Dr, 23.4m)
                }  ),
                new Journal(Guid.NewGuid(), new DateTime(2012,6,5),new DateTime(2012,6,5), new List<JournalLine>
                {
                    new JournalLine("9012", "Expenses", JournalType.Cr, 12.4m),
                    new JournalLine("3001", "Cash", JournalType.Dr, 12.4m)
                }  )
            };            
        }
    }
}
