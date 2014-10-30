using Searching;

namespace Audition.Session
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