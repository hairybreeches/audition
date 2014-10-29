using Model.Persistence;
using Model.Searching;

namespace Audition.Session
{
    public class LoginSession
    {
        private IJournalSearcherFactory searcherFactory;
        private InMemoryJournalRepository repository;

        public JournalSearcher GetCurrentJournalSearcher()
        {
            if (searcherFactory == null)
            {
                throw new NotLoggedInException();
            }
            return searcherFactory.CreateJournalSearcher(repository);
        }

        public void Login(IJournalSearcherFactory newSearcherFactory, InMemoryJournalRepository newRepository)
        {
            searcherFactory = newSearcherFactory;
            repository = newRepository;
        }

        public void Logout()
        {
            searcherFactory = null;            
        }
    }
}
