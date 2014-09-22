using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Audition.Chromium;
using Audition.Native;
using Excel;
using Model;
using Model.Accounting;
using Model.SearchWindows;

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
        public IEnumerable<Journal> HoursSearch(HoursSearchWindow searchWindow)
        {
            return searcher.FindJournalsWithin(searchWindow);
        }
        
        [HttpPost]
        [Route(Routing.HoursExport)]
        public async Task<IHttpActionResult> HoursExport(HoursSearchWindow saveRequest)
        {
            var journals = searcher.FindJournalsWithin(saveRequest);
            return await Export(journals);
        }

        private async Task<IHttpActionResult> Export(IEnumerable<Journal> journals)
        {
            var saveLocation = await fileSaveChooser.GetFileSaveLocation();
            excelExporter.WriteJournals(journals, saveLocation);
            return Ok(saveLocation);
        }
    }
}
