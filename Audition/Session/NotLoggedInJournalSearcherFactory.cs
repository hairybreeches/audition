using Persistence;
using Searching;

namespace Audition.Session
{
    public class NotLoggedInJournalSearcherFactory : IJournalSearcherFactory
    {
        public JournalSearcher CreateJournalSearcher(JournalRepository repository)
        {
            throw new NotLoggedInException();
        }
    }
}