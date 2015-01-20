using System.Linq;
using Model.Accounting;
using Model.SearchWindows;

namespace Searching
{
    public interface IJournalSearcher<T>
    {
        IQueryable<Journal> FindJournalsWithin(SearchWindow<T> searchWindow);
    }
}
