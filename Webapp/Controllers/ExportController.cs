using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Capabilities;
using CsvExport;
using Model;
using Model.Accounting;
using Native;
using Native.Dialogs;
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
        [Route(Routing.AccountsExport)]
        public async Task<ExportResult> AccountsExport(SearchWindow<UnusualAccountsParameters> searchWindow)
        {
            return await Export(searchWindow);
        }

        [HttpPost]
        [Route(Routing.EndingExport)]
        public async Task<ExportResult> EndingExport(SearchWindow<EndingParameters> searchWindow)
        {
            return await Export(searchWindow);
        }

        [HttpPost]
        [Route(Routing.UserExport)]
        public async Task<ExportResult> UserExport(SearchWindow<UserParameters> searchWindow)
        {
            return await Export(searchWindow);
        }

        private async Task<ExportResult> Export(ISearchWindow searchWindow)
        {
            var exportResult = await fileSaveChooser.GetFileSaveLocation();
            if (exportResult.Completed)
            {
                transactionExporter.Export(searchWindow.Description, searchWindow.Execute(Searcher, Repository).GetAllTransactions(), exportResult.Filename, new HashSet<DisplayField>(session.GetCurrentSearchCapability().AvailableFields));
            }
            return exportResult;
        }
    }
}