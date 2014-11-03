using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.SearchWindows;
using Persistence;

namespace Searching
{
    public class RoundNumberSearcher : IJournalSearcher<EndingParameters>
    {
        private readonly JournalRepository repository;

        public RoundNumberSearcher(JournalRepository repository)
        {
            this.repository = repository;
        }  

        public IQueryable<Journal> FindJournalsWithin(SearchWindow<EndingParameters> searchWindow)
        {
            var periodJournals = repository.GetJournalsApplyingTo(searchWindow.Period);
            var magnitude = searchWindow.Parameters.Magnitude();
            return periodJournals.Where(journal => HasRoundLine(journal, magnitude));
        }

        private bool HasRoundLine(Journal journal, int magnitude)
        {
            return journal.Lines.Any(line => ContainsRoundValue(line, magnitude));
        }

        private bool ContainsRoundValue(JournalLine line, int magnitude)
        {
            return IsRound(line.Amount, magnitude)
                   || IsRound(line.Amount, magnitude)
                   || IsRound(line.Amount, magnitude);
        }

        public bool IsRound(decimal amount, int magnitude)
        {
            var pence = amount * 100;
            return pence != 0 && pence % magnitude == 0;
        }               
    }
}