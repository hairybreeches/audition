using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;
using Raven.Client;

namespace Persistence
{
    public class JournalRepository
    {
        private readonly IDocumentSession session;

        public JournalRepository(IDocumentSession session)
        {
            this.session = session;
        }

        public IQueryable<Journal> GetJournalsApplyingTo(DateRange period)
        {
            return session.Query<Journal>().Where(x => period.Contains(x.JournalDate)).AsQueryable();
        }

        public JournalRepository UpdateJournals(IEnumerable<Journal> journals)
        {
            ClearJournals();
            foreach (var journal in journals)
            {
                session.Store(journal);
            }            
            session.SaveChanges();
            return this;
        }

        public void ClearJournals()
        {
            
        }
    }
}
