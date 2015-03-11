using System;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;

namespace Searching.SearchWindows
{
    public interface ISearchParameters
    {
        IQueryable<Transaction> ApplyFilter(Searcher searcher, IQueryable<Transaction> transactions);
        string Description { get; }
    }
}