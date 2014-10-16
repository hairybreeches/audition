using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.SearchWindows;
using Model.Time;

namespace Model.Searching
{
    public class WorkingHoursSearcher : IJournalSearcher<WorkingHours>
    {
        private readonly JournalRepository repository;

        public WorkingHoursSearcher(JournalRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<WorkingHours> searchWindow)
        {
            var periodJournals = repository.GetJournalsApplyingTo(searchWindow.Period).ToList();

            return periodJournals.Where(x => Matches(x, searchWindow.Parameters));
        }

        private static bool Matches(Journal x, WorkingHours workingHours)
        {
            return !workingHours.Contains(x.Created);
        } 

    }
}