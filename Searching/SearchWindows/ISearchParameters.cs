using System;
using System.Linq;
using Model.Accounting;
using Model.Time;

namespace Searching.SearchWindows
{
    public interface ISearchParameters
    {
        Func<DateRange, IQueryable<Journal>> GetSearchMethod(JournalSearcher searcher);
    }
}