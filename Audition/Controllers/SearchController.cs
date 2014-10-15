using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Audition.Chromium;
using Audition.Native;
using Audition.Session;
using Excel;
using Model;
using Model.Accounting;
using Model.SearchWindows;
using Model.Time;

namespace Audition.Controllers
{
    public class SearchController : ApiController
    {
        private IJournalSearcher Searcher
        {
            get { return session.GetCurrentJournalSearcher(); }
        }

        private readonly ExcelExporter excelExporter;
        private readonly IFileSaveChooser fileSaveChooser;
        private readonly LoginSession session;

        public SearchController(LoginSession session, ExcelExporter excelExporter, IFileSaveChooser fileSaveChooser)
        {
            this.session = session;
            this.excelExporter = excelExporter;
            this.fileSaveChooser = fileSaveChooser;
        }

        [HttpPost]
        [Route(Routing.HoursSearch)]
        public IEnumerable<Journal> HoursSearch(SearchWindow<WorkingHours> searchWindow)
        {
            return Searcher.FindJournalsWithin(searchWindow);
        }
        
        [HttpPost]
        [Route(Routing.HoursExport)]
        public async Task<IHttpActionResult> HoursExport(ExportRequest<WorkingHours> saveRequest)
        {
            var journals = Searcher.FindJournalsWithin(saveRequest.SearchWindow);
            return await Export(journals, saveRequest.SerialisationOptions);
        }      
        
        [HttpPost]
        [Route(Routing.AccountsSearch)]
        public IEnumerable<Journal> AccountsSearch(SearchWindow<UnusualAccountsParameters> searchWindow)
        {
            return Searcher.FindJournalsWithin(searchWindow);
        }
        
        [HttpPost]
        [Route(Routing.AccountsExport)]
        public async Task<IHttpActionResult> AccountsExport(ExportRequest<UnusualAccountsParameters> saveRequest)
        {
            var journals = Searcher.FindJournalsWithin(saveRequest.SearchWindow);
            return await Export(journals, saveRequest.SerialisationOptions);
        }    
        
        [HttpPost]
        [Route(Routing.DateSearch)]
        public IEnumerable<Journal> DateSearch(SearchWindow<YearEndParameters> searchWindow)
        {
            return Searcher.FindJournalsWithin(searchWindow);
        }
        
        [HttpPost]
        [Route(Routing.DateExport)]
        public async Task<IHttpActionResult> DateExport(ExportRequest<YearEndParameters> saveRequest)
        {
            var journals = Searcher.FindJournalsWithin(saveRequest.SearchWindow);
            return await Export(journals, saveRequest.SerialisationOptions);
        }

        [HttpPost]
        [Route(Routing.UserSearch)]
        public IEnumerable<Journal> UserSearch(SearchWindow<UserParameters> searchWindow)
        {
            return Searcher.FindJournalsWithin(searchWindow);
        }
        
        [HttpPost]
        [Route(Routing.UserExport)]
        public async Task<IHttpActionResult> UserExport(ExportRequest<UserParameters> saveRequest)
        {
            var journals = Searcher.FindJournalsWithin(saveRequest.SearchWindow);
            return await Export(journals, saveRequest.SerialisationOptions);
        } 
        
        [HttpPost]
        [Route(Routing.KeywordSearch)]
        public IEnumerable<Journal> KeywordSearch(SearchWindow<KeywordParameters> searchWindow)
        {
            return Searcher.FindJournalsWithin(searchWindow);
        }
        
        [HttpPost]
        [Route(Routing.KeywordExport)]
        public async Task<IHttpActionResult> KeywordExport(ExportRequest<KeywordParameters> saveRequest)
        {
            var journals = Searcher.FindJournalsWithin(saveRequest.SearchWindow);
            return await Export(journals, saveRequest.SerialisationOptions);
        }     
        
        [HttpPost]
        [Route(Routing.EndingSearch)]
        public IEnumerable<Journal> EndingSearch(SearchWindow<EndingParameters> searchWindow)
        {
            return Searcher.FindJournalsWithin(searchWindow);
        }
        
        [HttpPost]
        [Route(Routing.EndingExport)]
        public async Task<IHttpActionResult> EndingExport(ExportRequest<EndingParameters> saveRequest)
        {
            var journals = Searcher.FindJournalsWithin(saveRequest.SearchWindow);
            return await Export(journals, saveRequest.SerialisationOptions);
        }

        private async Task<IHttpActionResult> Export(IEnumerable<Journal> journals, SerialisationOptions options)
        {
            var saveLocation = await fileSaveChooser.GetFileSaveLocation();
            excelExporter.WriteJournals(journals, saveLocation, options);
            return Ok(saveLocation);
        }
    }
}
