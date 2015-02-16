using Searching;

namespace Webapp.Session
{
    public class JournalSearcherFactoryStorage
    {
        public JournalSearcherFactoryStorage()
        {
            CurrentSearcherFactory = new NoImportedDataSearcherFactory();
        }

        public ISearcherFactory CurrentSearcherFactory { get; set; }
    }
}