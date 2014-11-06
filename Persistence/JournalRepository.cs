using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;

namespace Persistence
{
    public class JournalRepository
    {
        private IEnumerable<Journal> Journals { get; set; }

        public JournalRepository()
        {
            Journals = Enumerable.Empty<Journal>();
        }

        public IQueryable<Journal> GetJournalsApplyingTo(DateRange period)
        {
            return Journals.ToList().Where(x => period.Contains(x.JournalDate)).AsQueryable();
        }

        public JournalRepository UpdateJournals(IEnumerable<Journal> journals)
        {
            Journals = journals;
            return this;
        }

        public void ClearJournals()
        {
            Journals = Enumerable.Empty<Journal>();
        }
    }
}
