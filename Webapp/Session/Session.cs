using System.Collections.Generic;
using Licensing;
using Model.Accounting;
using Persistence;
using Searching;
using Webapp.Controllers;

namespace Webapp.Session
{
    public class Session
    {
        private readonly SearcherFactoryStorage searcherFactoryStorage;
        private readonly ITransactionRepository repository;
        private readonly ILicenceStorage licenceStorage;

        public Session(SearcherFactoryStorage searcherFactoryStorage, ITransactionRepository repository, ILicenceStorage licenceStorage)
        {
            this.searcherFactoryStorage = searcherFactoryStorage;
            this.repository = repository;
            this.licenceStorage = licenceStorage;
        }

        public Searcher GetCurrentJournalSearcher()
        {            
            licenceStorage.EnsureUseAllowed();
            return SearcherFactory.CreateSearcher();
        }

        public void ImportData(ISearcherFactory newSearcherFactory, IEnumerable<Transaction> transactions)
        {
            searcherFactoryStorage.CurrentSearcherFactory = newSearcherFactory;
            repository.UpdateTransactions(transactions);
        }

        public void ClearImport()
        {
            searcherFactoryStorage.CurrentSearcherFactory = new NoImportedDataSearcherFactory();
            repository.ClearTransactions();
        }

        public ITransactionRepository Repository { get {  return repository; } }

        public SearchCapability GetCurrentSearchCapability()
        {
            return SearcherFactory.GetSearchCapability();
        }

        private ISearcherFactory SearcherFactory
        {
            get { return searcherFactoryStorage.CurrentSearcherFactory; }
        }
    }
}
