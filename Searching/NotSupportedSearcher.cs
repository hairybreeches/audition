using System;
using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Searching.SearchWindows;

namespace Searching
{
    public class NotSupportedSearcher<T> : IJournalSearcher<T> where T : ISearchParameters
    {
        private readonly string errorMessage;

        public NotSupportedSearcher(string errorMessage)
        {
            this.errorMessage = errorMessage;
        }

        public IQueryable<Journal> FindJournalsWithin(SearchWindow<T> searchWindow)
        {            
            throw new NotSupportedException(errorMessage);
        }
    }
}