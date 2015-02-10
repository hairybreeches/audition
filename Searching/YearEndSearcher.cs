using System;
using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;
using Searching.SearchWindows;

namespace Searching
{
    public class YearEndSearcher : IJournalSearcher<YearEndParameters>
    {
        public IQueryable<Journal> FindJournalsWithin(YearEndParameters parameters, IQueryable<Journal> journals)
        {            
            var startOfSearchPeriod = GetCreationStartDate(parameters);
            return journals.Where(x => x.Created >= startOfSearchPeriod);            
        }

        private static DateTimeOffset GetCreationStartDate(YearEndParameters parameters)
        {            
            return parameters.YearEnd.Subtract(TimeSpan.FromDays(parameters.DaysBeforeYearEnd));
        }
    }
}