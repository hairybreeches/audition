using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using Excel;
using Model;

namespace Audition.Controllers
{
    public class SearchController : ApiController
    {
        private readonly IJournalSearcher searcher;
        private readonly ExcelExporter excelExporter;

        public SearchController(IJournalSearcher searcher, ExcelExporter excelExporter)
        {
            this.searcher = searcher;
            this.excelExporter = excelExporter;
        }

        [HttpGet]
        [Route("api/search")]
        public IEnumerable<Journal> Search([ModelBinder(typeof(JsonModelBinder))] SearchWindow searchWindow)
        {
            return searcher.FindJournalsWithin(searchWindow);
        }
        
        [HttpPost]
        [Route("api/search/export")]
        public void Export([ModelBinder(typeof(JsonModelBinder))] SaveSearchRequest saveRequest)
        {
            var journals = searcher.FindJournalsWithin(saveRequest.SearchWindow);
            excelExporter.WriteJournals(journals, saveRequest.Filename);
        }
    }
}
