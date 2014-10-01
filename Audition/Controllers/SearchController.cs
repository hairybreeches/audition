using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Audition.Chromium;
using Audition.Native;
using Excel;
using Model;
using Model.Accounting;
using Model.SearchWindows;
using Model.Time;

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
        public IEnumerable<Journal> HoursSearch(SearchWindow<WorkingHours> searchWindow)
        {
            return searcher.FindJournalsWithin(searchWindow);
        }
        
        [HttpPost]
        [Route(Routing.HoursExport)]
        public async Task<IHttpActionResult> HoursExport(SearchWindow<WorkingHours> saveRequest)
        {
            var journals = searcher.FindJournalsWithin(saveRequest);
            return await Export(journals);
        }      
        
        [HttpPost]
        [Route(Routing.AccountsSearch)]
        public IEnumerable<Journal> AccountsSearch(SearchWindow<UnusualAccountsParameters> searchWindow)
        {
            return searcher.FindJournalsWithin(searchWindow);
        }
        
        [HttpPost]
        [Route(Routing.AccountsExport)]
        public async Task<IHttpActionResult> AccountsExport(SearchWindow<UnusualAccountsParameters> saveRequest)
        {
            var journals = searcher.FindJournalsWithin(saveRequest);
            return await Export(journals);
        }    
        
        [HttpPost]
        [Route(Routing.DateSearch)]
        public IEnumerable<Journal> DateSearch(SearchWindow<YearEndParameters> searchWindow)
        {
            return searcher.FindJournalsWithin(searchWindow);
        }
        
        [HttpPost]
        [Route(Routing.DateExport)]
        public async Task<IHttpActionResult> DateExport(SearchWindow<YearEndParameters> saveRequest)
        {
            var journals = searcher.FindJournalsWithin(saveRequest);
            return await Export(journals);
        }

        [HttpPost]
        [Route(Routing.UserSearch)]
        public IEnumerable<Journal> UserSearch(SearchWindow<UserParameters> searchWindow)
        {
            return searcher.FindJournalsWithin(searchWindow);
        }
        
        [HttpPost]
        [Route(Routing.UserExport)]
        public async Task<IHttpActionResult> UserExport(SearchWindow<UserParameters> saveRequest)
        {
            var journals = searcher.FindJournalsWithin(saveRequest);
            return await Export(journals);
        } 
        
        [HttpPost]
        [Route(Routing.KeywordSearch)]
        public IEnumerable<Journal> KeywordSearch(SearchWindow<KeywordParameters> searchWindow)
        {
            return searcher.FindJournalsWithin(searchWindow);
        }
        
        [HttpPost]
        [Route(Routing.KeywordExport)]
        public async Task<IHttpActionResult> KeywordExport(SearchWindow<KeywordParameters> saveRequest)
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
