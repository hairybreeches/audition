using System.Linq;
using Model.Accounting;

namespace Searching
{
    public interface ISearchWindow
    {
        IQueryable<Journal> Execute(JournalSearcher searcher);
        string Description { get; }
    }
}