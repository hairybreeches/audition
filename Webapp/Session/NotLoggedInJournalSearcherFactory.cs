using Persistence;
using Searching;

namespace Webapp.Session
{
    public class NotLoggedInJournalSearcherFactory : IJournalSearcherFactory
    {
        public JournalSearcher CreateJournalSearcher()
        {
            throw new NotLoggedInException();
        }

        public SearchCapability GetSearchCapability()
        {
            throw new NotLoggedInException();
        }
    }
}