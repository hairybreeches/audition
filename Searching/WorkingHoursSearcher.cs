using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;
using Searching.SearchWindows;

namespace Searching
{
    public class WorkingHoursSearcher : IJournalSearcher<WorkingHoursParameters>
    {
        public IQueryable<Transaction> FindJournalsWithin(WorkingHoursParameters parameters, IQueryable<Transaction> journals)
        {            
            return journals.Where(x => Matches(x, parameters));
        }

        private static bool Matches(Transaction x, WorkingHoursParameters workingHours)
        {
            return !workingHours.Contains(x.Created);
        } 

    }
}