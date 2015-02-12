using Persistence;
using Searching;

namespace Webapp.Session
{
    public class NotLoggedInJournalSearcherFactory : IJournalSearcherFactory
    {
        public JournalSearcher CreateJournalSearcher()
        {
            throw new NoImportedDataException();
        }

        public SearchCapability GetSearchCapability()
        {
            throw new NoImportedDataException();
        }
    }
}