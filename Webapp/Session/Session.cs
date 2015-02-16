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
        private readonly JournalSearcherFactoryStorage searcherFactoryStorage;
        private readonly IJournalRepository repository;
        private readonly ILicenceStorage licenceStorage;

        public Session(JournalSearcherFactoryStorage searcherFactoryStorage, IJournalRepository repository, ILicenceStorage licenceStorage)
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

        public void ImportData(ISearcherFactory newSearcherFactory, IEnumerable<Transaction> journals)
        {
            searcherFactoryStorage.CurrentSearcherFactory = newSearcherFactory;
            repository.UpdateJournals(journals);
        }

        public void ClearImport()
        {
            searcherFactoryStorage.CurrentSearcherFactory = new NoImportedDataSearcherFactory();
            repository.ClearJournals();
        }

        public IJournalRepository Repository { get {  return repository; } }

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
