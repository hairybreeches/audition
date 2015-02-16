using System.Linq;
using Model.Accounting;
using Persistence;

namespace Searching
{
    public interface ISearchWindow
    {
        IQueryable<Transaction> Execute(JournalSearcher searcher, IJournalRepository repository);
        string Description { get; }
    }
}