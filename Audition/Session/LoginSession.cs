using Model.Persistence;
using Model.Searching;

namespace Audition.Session
{
    public class LoginSession
    {
        private IJournalSearcherFactory searcherFactory;

        public JournalSearcher GetCurrentJournalSearcher(JournalRepository repository)
        {
            if (searcherFactory == null)
            {
                throw new NotLoggedInException();
            }
            return searcherFactory.CreateJournalSearcher(repository);
        }

        public void Login(IJournalSearcherFactory newSearcherFactory)
        {
            searcherFactory = newSearcherFactory;
        }

        public void Logout()
        {
            searcherFactory = null;            
        }
    }
}
