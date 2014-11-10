using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;

namespace Persistence
{
    public class InMemoryJournalRepository : IJournalRepository
    {
        private IEnumerable<Journal> Journals { get; set; }

        public InMemoryJournalRepository()
        {
            Journals = Enumerable.Empty<Journal>();
        }

        public IQueryable<Journal> GetJournalsApplyingTo(DateRange period)
        {
            return Journals.ToList().Where(x => period.Contains(x.JournalDate)).AsQueryable();
        }

        public IJournalRepository UpdateJournals(IEnumerable<Journal> journals)
        {
            Journals = journals.ToList();
            return this;
        }

        public void ClearJournals()
        {
            Journals = Enumerable.Empty<Journal>();
        }
    }
}
