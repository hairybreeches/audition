using Model.Searching;

namespace Audition.Session
{
    public class LoginSession
    {
        private JournalSearcher searcher;

        public JournalSearcher GetCurrentJournalSearcher()
        {
            if (searcher == null)
            {
                throw new NotLoggedInException();
            }
            return searcher;
        }

        public void Login(JournalSearcher journalSearcher)
        {
            searcher = journalSearcher;
        }

        public void Logout()
        {
            searcher = null;            
        }
    }
}
