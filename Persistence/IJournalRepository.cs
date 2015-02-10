using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;

namespace Persistence
{
    public interface IJournalRepository
    {
        IQueryable<Journal> GetJournals();
        IJournalRepository UpdateJournals(IEnumerable<Journal> journals);
        void ClearJournals();
    }
}