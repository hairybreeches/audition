using System;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;

namespace Searching.SearchWindows
{
    public interface ISearchParameters
    {
        IQueryable<Journal> ApplyFilter(JournalSearcher searcher, IQueryable<Journal> journals);
    }
}