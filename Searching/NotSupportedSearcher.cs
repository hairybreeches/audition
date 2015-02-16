using System;
using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;
using Searching.SearchWindows;

namespace Searching
{
    public class NotSupportedSearcher<T> : ISearcher<T> where T : ISearchParameters
    {
        private readonly string errorMessage;

        public NotSupportedSearcher(string errorMessage)
        {
            this.errorMessage = errorMessage;
        }

        public IQueryable<Transaction> FindTransactionsWithin(T parameters, IQueryable<Transaction> journals)
        {
            throw new NotSupportedException(errorMessage);
        }
    }
}