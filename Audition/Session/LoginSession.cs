using System.Collections.Generic;
using Model.Accounting;
using Persistence;
using Searching;

namespace Audition.Session
{
    public class LoginSession
    {
        private readonly JournalSearcherFactoryStorage searcherFactoryStorage;
        private JournalRepository repository;

        public LoginSession(JournalSearcherFactoryStorage searcherFactoryStorage)
        {
            this.searcherFactoryStorage = searcherFactoryStorage;            
        }

        public JournalSearcher GetCurrentJournalSearcher()
        {            
            return searcherFactoryStorage.CurrentSearcherFactory.CreateJournalSearcher(repository);
        }

        public void Login(IJournalSearcherFactory newSearcherFactory, IEnumerable<Journal> journals)
        {
            searcherFactoryStorage.CurrentSearcherFactory = newSearcherFactory;
            repository = new JournalRepository(journals);
        }

        public void Logout()
        {
            searcherFactoryStorage.CurrentSearcherFactory = new NotLoggedInJournalSearcherFactory();            
        }
    }
}
