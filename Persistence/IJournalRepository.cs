using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;

namespace Persistence
{
    public interface IJournalRepository
    {
        IQueryable<Transaction> GetJournals();
        IJournalRepository UpdateJournals(IEnumerable<Transaction> journals);
        void ClearJournals();
    }
}