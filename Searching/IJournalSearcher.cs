using System.Linq;
using Model.Accounting;
using Model.SearchWindows;

namespace Searching
{
    public interface IJournalSearcher<T> where T : ISearchParameters
    {
        IQueryable<Journal> FindJournalsWithin(SearchWindow<T> searchWindow);
    }
}
