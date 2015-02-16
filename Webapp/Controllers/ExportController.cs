using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using CsvExport;
using Model.Accounting;
using Native;
using Persistence;
using Searching;
using Searching.SearchWindows;
using Webapp.Session;

namespace Webapp.Controllers
{
    public class ExportController : ApiController
    {
        private readonly Session.Session session;
        private readonly ITransactionExporter transactionExporter;
        private readonly IFileSaveChooser fileSaveChooser;

        private Searcher Searcher
        {
            get { return session.GetCurrentJournalSearcher(); }
        }    
        
        private ITransactionRepository Repository
        {
            get { return session.Repository; }
        }

        public ExportController(IFileSaveChooser fileSaveChooser, ITransactionExporter transactionExporter, Session.Session session)
        {
            this.fileSaveChooser = fileSaveChooser;
            this.transactionExporter = transactionExporter;
            this.session = session;
        }

        [HttpPost]
        [Route(Routing.HoursExport)]
        public async Task<string> HoursExport(SearchWindow<WorkingHoursParameters> searchWindow)
        {
            return await Export(searchWindow);
        }

        [HttpPost]
        [Route(Routing.AccountsExport)]
        public async Task<string> AccountsExport(SearchWindow<UnusualAccountsParameters> searchWindow)
        {
            return await Export(searchWindow);
        }

        [HttpPost]
        [Route(Routing.DateExport)]
        public async Task<string> DateExport(SearchWindow<YearEndParameters> searchWindow)
        {
            return await Export(searchWindow);
        }

        [HttpPost]
        [Route(Routing.EndingExport)]
        public async Task<string> EndingExport(SearchWindow<EndingParameters> searchWindow)
        {
            return await Export(searchWindow);
        }

        [HttpPost]
        [Route(Routing.UserExport)]
        public async Task<string> UserExport(SearchWindow<UserParameters> searchWindow)
        {
            return await Export(searchWindow);
        }

        private async Task<string> Export(ISearchWindow searchWindow)
        {
            var saveLocation = await fileSaveChooser.GetFileSaveLocation();
            transactionExporter.WriteTransactions(searchWindow.Description, searchWindow.Execute(Searcher, Repository).GetAllTransactions(), saveLocation, session.GetCurrentSearchCapability().AvailableFields);
            return saveLocation;
        }
    }
}