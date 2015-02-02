using Persistence;

namespace Searching
{
    public interface IJournalSearcherFactory
    {
        JournalSearcher CreateJournalSearcher(IJournalRepository repository);
        SearchCapability GetSearchCapability();
    }
}