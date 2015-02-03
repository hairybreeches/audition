using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CsvExport;
using Model.Accounting;
using Model.SearchWindows;
using Native;
using Persistence;
using Searching;
using Webapp.Requests;
using Webapp.Session;

namespace Webapp.Controllers
{
    public class ExportController : ApiController
    {
        private readonly LoginSession session;
        private readonly ICsvExporter csvExporter;
        private readonly IFileSaveChooser fileSaveChooser;

        private JournalSearcher Searcher
        {
            get { return session.GetCurrentJournalSearcher(); }
        }

        public ExportController(IFileSaveChooser fileSaveChooser, ICsvExporter csvExporter, LoginSession session)
        {
            this.fileSaveChooser = fileSaveChooser;
            this.csvExporter = csvExporter;
            this.session = session;
        }

        [HttpPost]
        [Route(Routing.HoursExport)]
        public async Task<string> HoursExport(ExportRequest<WorkingHoursParameters> saveRequest)
        {
            return await Export(saveRequest, Searcher.FindJournalsWithin);
        }

        [HttpPost]
        [Route(Routing.AccountsExport)]
        public async Task<string> AccountsExport(ExportRequest<UnusualAccountsParameters> saveRequest)
        {
            return await Export(saveRequest, Searcher.FindJournalsWithin);
        }

        [HttpPost]
        [Route(Routing.DateExport)]
        public async Task<string> DateExport(ExportRequest<YearEndParameters> saveRequest)
        {
            return await Export(saveRequest, Searcher.FindJournalsWithin);
        }

        [HttpPost]
        [Route(Routing.EndingExport)]
        public async Task<string> EndingExport(ExportRequest<EndingParameters> saveRequest)
        {
            return await Export(saveRequest, Searcher.FindJournalsWithin);
        }

        [HttpPost]
        [Route(Routing.UserExport)]
        public async Task<string> UserExport(ExportRequest<UserParameters> saveRequest)
        {
            return await Export(saveRequest, Searcher.FindJournalsWithin);
        }

        private async Task<string> Export<T>(ExportRequest<T> saveRequest, Func<SearchWindow<T>, IQueryable<Journal>> searchMethod)
        {
            var saveLocation = await fileSaveChooser.GetFileSaveLocation();
            csvExporter.WriteJournals(saveRequest.SearchWindow.Description, searchMethod(saveRequest.SearchWindow).GetAllJournals(), saveLocation);
            return saveLocation;
        }
    }
}