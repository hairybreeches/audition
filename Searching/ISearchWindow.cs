using System.Linq;
using Model.Accounting;
using Persistence;

namespace Searching
{
    public interface ISearchWindow
    {
        IQueryable<Transaction> Execute(Searcher searcher, ITransactionRepository repository);
        string Description { get; }
    }
}