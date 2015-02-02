using Searching;

namespace Webapp.Session
{
    public class JournalSearcherFactoryStorage
    {
        public JournalSearcherFactoryStorage()
        {
            CurrentSearcherFactory = new NotLoggedInJournalSearcherFactory();
        }

        public IJournalSearcherFactory CurrentSearcherFactory { get; set; }
    }
}