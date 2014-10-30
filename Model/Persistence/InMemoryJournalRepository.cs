using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;

namespace Model.Persistence
{
    public class InMemoryJournalRepository
    {
        private IEnumerable<Journal> Journals { get; set; }

        public InMemoryJournalRepository(IEnumerable<Journal> journals)
        {
            Journals = journals.ToList();
        }

        public IEnumerable<Journal> GetJournalsApplyingTo(DateRange period)
        {
            return Journals.ToList().Where(x => period.Contains(x.JournalDate));
        }
    }
}
