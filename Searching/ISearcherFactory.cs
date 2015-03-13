using System.Collections.Generic;
using Capabilities;
using Persistence;

namespace Searching
{
    public interface ISearcherFactory
    {
        Searcher CreateSearcher();
        SearchCapability GetSearchCapability();
        IEnumerable<DisplayField> GetAvailableFields();
    }
}