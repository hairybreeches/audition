using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Audition.Chromium;
using Audition.Native;
using Excel;
using Model;

namespace Audition.Controllers
{
    public class SearchController : ApiController
    {
        private readonly IJournalSearcher searcher;
        private readonly ExcelExporter excelExporter;
        private readonly IFileSaveChooser fileSaveChooser;

        public SearchController(IJournalSearcher searcher, ExcelExporter excelExporter, IFileSaveChooser fileSaveChooser)
        {
            this.searcher = searcher;
            this.excelExporter = excelExporter;
            this.fileSaveChooser = fileSaveChooser;
        }

        [HttpPost]
        [Route(Routing.HoursSearch)]
        public IEnumerable<Journal> Search(SearchWindow searchWindow)
        {
            return searcher.FindJournalsWithin(searchWindow);
        }
        
        [HttpPost]
        [Route(Routing.HoursExport)]
        public async Task<IHttpActionResult> Export(SearchWindow saveRequest)
        {
            var saveLocation = await fileSaveChooser.GetFileSaveLocation();
            var journals = searcher.FindJournalsWithin(saveRequest);
            excelExporter.WriteJournals(journals, saveLocation);
            return Ok(saveLocation);
        }
    }
}
