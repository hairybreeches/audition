using System.Collections.Generic;
using Model.Accounting;
using Persistence;
using Searching;

namespace Webapp.Session
{
    public class LoginSession
    {
        private readonly JournalSearcherFactoryStorage searcherFactoryStorage;
        private readonly IJournalRepository repository;

        public LoginSession(JournalSearcherFactoryStorage searcherFactoryStorage, IJournalRepository repository)
        {
            this.searcherFactoryStorage = searcherFactoryStorage;
            this.repository = repository;
        }

        public JournalSearcher GetCurrentJournalSearcher()
        {            
            return searcherFactoryStorage.CurrentSearcherFactory.CreateJournalSearcher(repository);
        }

        public void Login(IJournalSearcherFactory newSearcherFactory, IEnumerable<Journal> journals)
        {
            searcherFactoryStorage.CurrentSearcherFactory = newSearcherFactory;
            repository.UpdateJournals(journals);
        }

        public void Logout()
        {
            searcherFactoryStorage.CurrentSearcherFactory = new NotLoggedInJournalSearcherFactory();
            repository.ClearJournals();
        }
    }
}
