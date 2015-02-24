using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Persistence;
using Searching.SearchWindows;

namespace Searching
{
    public class WorkingHoursSearcher : ISearcher<WorkingHoursParameters>
    {
        public IQueryable<Transaction> FindTransactionsWithin(WorkingHoursParameters parameters, IQueryable<Transaction> transactions)
        {            
            return transactions.Where(x => Matches(x, parameters));
        }

        private static bool Matches(Transaction x, WorkingHoursParameters workingHours)
        {
            return !workingHours.Contains(x.Created);
        } 

    }
}