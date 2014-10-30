using System.Collections.Generic;
using Model.Accounting;
using Model.SearchWindows;

namespace Searching
{
    public interface IJournalSearcher<T>
    {
        IEnumerable<Journal> FindJournalsWithin(SearchWindow<T> searchWindow);
    }
}
