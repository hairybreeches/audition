using Model.Persistence;
using Searching;

namespace Audition.Session
{
    public class LoginSession
    {
        private IJournalSearcherFactory searcherFactory;
        private JournalRepository repository;

        public JournalSearcher GetCurrentJournalSearcher()
        {
            if (searcherFactory == null)
            {
                throw new NotLoggedInException();
            }
            return searcherFactory.CreateJournalSearcher(repository);
        }

        public void Login(IJournalSearcherFactory newSearcherFactory, JournalRepository newRepository)
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
