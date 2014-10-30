using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.SearchWindows;
using Persistence;

namespace Searching
{
    public class WorkingHoursSearcher : IJournalSearcher<WorkingHoursParameters>
    {
        private readonly JournalRepository repository;

        public WorkingHoursSearcher(JournalRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<WorkingHoursParameters> searchWindow)
        {
            var periodJournals = repository.GetJournalsApplyingTo(searchWindow.Period).ToList();

            return periodJournals.Where(x => Matches(x, searchWindow.Parameters));
        }

        private static bool Matches(Journal x, WorkingHoursParameters workingHours)
        {
            return !workingHours.Contains(x.Created);
        } 

    }
}