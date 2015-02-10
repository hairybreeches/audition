using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;
using Searching.SearchWindows;

namespace Searching
{
    public interface IJournalSearcher<in T> where T : ISearchParameters
    {
        IQueryable<Journal> FindJournalsWithin(T parameters, DateRange dateRange, IJournalRepository repository);
    }
}
