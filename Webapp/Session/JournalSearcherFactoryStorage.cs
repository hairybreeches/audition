using Searching;

namespace Webapp.Session
{
    public class JournalSearcherFactoryStorage
    {
        public JournalSearcherFactoryStorage()
        {
            CurrentSearcherFactory = new NoImportedDataJournalSearcherFactory();
        }

        public IJournalSearcherFactory CurrentSearcherFactory { get; set; }
    }
}