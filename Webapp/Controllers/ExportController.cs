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
using Webapp.Session;

namespace Webapp.Controllers
{
    public class ExportController : ApiController
    {
        private readonly LoginSession session;
        private readonly IJournalExporter journalExporter;
        private readonly IFileSaveChooser fileSaveChooser;

        private JournalSearcher Searcher
        {
            get { return session.GetCurrentJournalSearcher(); }
        }

        public ExportController(IFileSaveChooser fileSaveChooser, IJournalExporter journalExporter, LoginSession session)
        {
            this.fileSaveChooser = fileSaveChooser;
            this.journalExporter = journalExporter;
            this.session = session;
        }

        [HttpPost]
        [Route(Routing.HoursExport)]
        public async Task<string> HoursExport(SearchWindow<WorkingHoursParameters> searchWindow)
        {
            return await Export(searchWindow, Searcher.FindJournalsWithin);
        }

        [HttpPost]
        [Route(Routing.AccountsExport)]
        public async Task<string> AccountsExport(SearchWindow<UnusualAccountsParameters> searchWindow)
        {
            return await Export(searchWindow, Searcher.FindJournalsWithin);
        }

        [HttpPost]
        [Route(Routing.DateExport)]
        public async Task<string> DateExport(SearchWindow<YearEndParameters> searchWindow)
        {
            return await Export(searchWindow, Searcher.FindJournalsWithin);
        }

        [HttpPost]
        [Route(Routing.EndingExport)]
        public async Task<string> EndingExport(SearchWindow<EndingParameters> searchWindow)
        {
            return await Export(searchWindow, Searcher.FindJournalsWithin);
        }

        [HttpPost]
        [Route(Routing.UserExport)]
        public async Task<string> UserExport(SearchWindow<UserParameters> searchWindow)
        {
            return await Export(searchWindow, Searcher.FindJournalsWithin);
        }

        private async Task<string> Export<T>(SearchWindow<T> searchWindow, Func<SearchWindow<T>, IQueryable<Journal>> searchMethod)
        {
            var saveLocation = await fileSaveChooser.GetFileSaveLocation();
            journalExporter.WriteJournals(searchWindow.Description, searchMethod(searchWindow).GetAllJournals(), saveLocation);
            return saveLocation;
        }
    }
}