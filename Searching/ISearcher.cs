using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;
using Searching.SearchWindows;

namespace Searching
{
    public interface ISearcher<in T> where T : ISearchParameters
    {
        IQueryable<Transaction> FindJournalsWithin(T parameters, IQueryable<Transaction> journals);
    }
}
