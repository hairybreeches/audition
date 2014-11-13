using Persistence;
using Searching;

namespace Audition.Session
{
    public class NotLoggedInJournalSearcherFactory : IJournalSearcherFactory
    {
        public JournalSearcher CreateJournalSearcher(IJournalRepository repository)
        {
            throw new NotLoggedInException();
        }
    }
}