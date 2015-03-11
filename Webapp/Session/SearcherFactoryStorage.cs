using Searching;

namespace Webapp.Session
{
    public class SearcherFactoryStorage
    {
        public SearcherFactoryStorage()
        {
            CurrentSearcherFactory = new NoImportedDataSearcherFactory();
        }

        public ISearcherFactory CurrentSearcherFactory { get; set; }
    }
}