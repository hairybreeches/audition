using Model;
using Xero;

namespace Audition.Session
{
    public class LoginSession
    {
        private IJournalSearcher searcher;

        public IJournalSearcher GetCurrentJournalSearcher()
        {
            if (searcher == null)
            {
                throw new NotLoggedInException();
            }
            return searcher;
        }

        public void Login(IJournalSearcher journalSearcher)
        {
            searcher = journalSearcher;
        }

        public void Logout()
        {
            searcher = null;            
        }
    }
}
