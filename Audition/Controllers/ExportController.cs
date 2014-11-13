using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Audition.Chromium;
using Audition.Requests;
using Audition.Session;
using Excel;
using Model;
using Model.Accounting;
using Model.SearchWindows;
using Native;
using Persistence;
using Searching;

namespace Audition.Controllers
{
    public class ExportController : ApiController
    {
        private readonly LoginSession session;
        private readonly IExcelExporter excelExporter;
        private readonly IFileSaveChooser fileSaveChooser;

        private JournalSearcher Searcher
        {
            get { return session.GetCurrentJournalSearcher(); }
        }

        public ExportController(IFileSaveChooser fileSaveChooser, IExcelExporter excelExporter, LoginSession session)
        {
            this.fileSaveChooser = fileSaveChooser;
            this.excelExporter = excelExporter;
            this.session = session;
        }

        [HttpPost]
        [Route(Routing.HoursExport)]
        public async Task<string> HoursExport(ExportRequest<WorkingHoursParameters> saveRequest)
        {
            //todo: duplication!
            var journals = Searcher.FindJournalsWithin(saveRequest.SearchWindow);
            return await Export(saveRequest.SearchWindow.Description, journals, saveRequest.SerialisationOptions);
        }

        [HttpPost]
        [Route(Routing.AccountsExport)]
        public async Task<string> AccountsExport(ExportRequest<UnusualAccountsParameters> saveRequest)
        {
            var journals = Searcher.FindJournalsWithin(saveRequest.SearchWindow);
            return await Export(saveRequest.SearchWindow.Description, journals, saveRequest.SerialisationOptions);
        }

        [HttpPost]
        [Route(Routing.DateExport)]
        public async Task<string> DateExport(ExportRequest<YearEndParameters> saveRequest)
        {
            var journals = Searcher.FindJournalsWithin(saveRequest.SearchWindow);
            return await Export(saveRequest.SearchWindow.Description, journals, saveRequest.SerialisationOptions);
        }

        [HttpPost]
        [Route(Routing.EndingExport)]
        public async Task<string> EndingExport(ExportRequest<EndingParameters> saveRequest)
        {
            var journals = Searcher.FindJournalsWithin(saveRequest.SearchWindow);
            return await Export(saveRequest.SearchWindow.Description, journals, saveRequest.SerialisationOptions);
        }

        [HttpPost]
        [Route(Routing.UserExport)]
        public async Task<string> UserExport(ExportRequest<UserParameters> saveRequest)
        {
            var journals = Searcher.FindJournalsWithin(saveRequest.SearchWindow);
            return await Export(saveRequest.SearchWindow.Description, journals, saveRequest.SerialisationOptions);
        }

        private async Task<string> Export(string description, IQueryable<Journal> journals, SerialisationOptions options)
        {
            var saveLocation = await fileSaveChooser.GetFileSaveLocation();
            excelExporter.WriteJournals(description, journals.GetAllJournals(), saveLocation, options);
            return saveLocation;
        }
    }
}