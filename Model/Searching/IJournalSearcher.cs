using System.Collections.Generic;
using Model.Accounting;
using Model.SearchWindows;
using Model.Time;

namespace Model.Searching
{
    public interface IJournalSearcher<T>
    {
        IEnumerable<Journal> FindJournalsWithin(SearchWindow<T> searchWindow);
    }
}
