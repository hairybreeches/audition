using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Audition.Chromium;
using Audition.Native;
using Audition.Requests;
using Audition.Session;
using Excel;
using Model;
using Model.Accounting;
using Model.Searching;
using Model.SearchWindows;
using Model.Time;

namespace Audition.Controllers
{
    public class ExportController : ApiController
    {
        private readonly LoginSession session;
        private readonly ExcelExporter excelExporter;
        private readonly IFileSaveChooser fileSaveChooser;

        private IJournalSearcher Searcher
        {
            get { return session.GetCurrentJournalSearcher(); }
        }

        public ExportController(IFileSaveChooser fileSaveChooser, ExcelExporter excelExporter, LoginSession session)
        {
            this.fileSaveChooser = fileSaveChooser;
            this.excelExporter = excelExporter;
            this.session = session;
        }

        [HttpPost]
        [Route(Routing.HoursExport)]
        public async Task<IHttpActionResult> HoursExport(ExportRequest<WorkingHours> saveRequest)
        {
            //todo: duplication!
            var journals = Searcher.FindJournalsWithin(saveRequest.SearchWindow);
            return await Export(journals, saveRequest.SerialisationOptions);
        }

        [HttpPost]
        [Route(Routing.AccountsExport)]
        public async Task<IHttpActionResult> AccountsExport(ExportRequest<UnusualAccountsParameters> saveRequest)
        {
            var journals = Searcher.FindJournalsWithin(saveRequest.SearchWindow);
            return await Export(journals, saveRequest.SerialisationOptions);
        }

        [HttpPost]
        [Route(Routing.DateExport)]
        public async Task<IHttpActionResult> DateExport(ExportRequest<YearEndParameters> saveRequest)
        {
            var journals = Searcher.FindJournalsWithin(saveRequest.SearchWindow);
            return await Export(journals, saveRequest.SerialisationOptions);
        }

        [HttpPost]
        [Route(Routing.EndingExport)]
        public async Task<IHttpActionResult> EndingExport(ExportRequest<EndingParameters> saveRequest)
        {
            var journals = Searcher.FindJournalsWithin(saveRequest.SearchWindow);
            return await Export(journals, saveRequest.SerialisationOptions);
        }

        [HttpPost]
        [Route(Routing.UserExport)]
        public async Task<IHttpActionResult> UserExport(ExportRequest<UserParameters> saveRequest)
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