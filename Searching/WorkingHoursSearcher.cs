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
        private readonly IJournalRepository repository;

        public WorkingHoursSearcher(IJournalRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<Journal> FindJournalsWithin(WorkingHoursParameters parameters, DateRange dateRange)
        {
            var periodJournals = repository.GetJournalsApplyingTo(dateRange);

            return periodJournals.Where(x => Matches(x, parameters));
        }

        private static bool Matches(Journal x, WorkingHoursParameters workingHours)
        {
            return !workingHours.Contains(x.Created);
        } 

    }
}