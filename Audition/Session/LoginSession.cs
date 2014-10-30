using Persistence;
using Searching;

namespace Audition.Session
{
    public class LoginSession
    {
        private IJournalSearcherFactory searcherFactory;
        private JournalRepository repository;

        public LoginSession()
        {
            searcherFactory = new NotLoggedInJournalSearcherFactory();
        }

        public JournalSearcher GetCurrentJournalSearcher()
        {            
            return searcherFactory.CreateJournalSearcher(repository);
        }

        public void Login(IJournalSearcherFactory newSearcherFactory, JournalRepository newRepository)
        {
            searcherFactory = newSearcherFactory;
            repository = newRepository;
        }

        public void Logout()
        {
            searcherFactory = new NotLoggedInJournalSearcherFactory();            
        }
    }
}
