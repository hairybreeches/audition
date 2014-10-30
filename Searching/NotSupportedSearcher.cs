using System;
using System.Collections.Generic;
using Model.Accounting;
using Model.SearchWindows;

namespace Searching
{
    public class NotSupportedSearcher<T> : IJournalSearcher<T>
    {
        private readonly string errorMessage;

        public NotSupportedSearcher(string errorMessage)
        {
            this.errorMessage = errorMessage;
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<T> searchWindow)
        {            
            throw new NotSupportedException(errorMessage);
        }
    }
}