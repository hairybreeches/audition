using Persistence;

namespace Searching
{
    public interface ISearcherFactory
    {
        Searcher CreateSearcher();
        SearchCapability GetSearchCapability();
    }
}