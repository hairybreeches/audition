using System.Linq;
using Model.Accounting;
using Persistence;

namespace Searching
{
    public interface ISearchWindow
    {
        IQueryable<Journal> Execute(JournalSearcher searcher, IJournalRepository repository);
        string Description { get; }
    }
}