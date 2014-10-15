using System;
using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.SearchWindows;
using Model.Time;

namespace Model.Searching
{
    public class JournalSearcher : IJournalSearcher
    {
        private readonly JournalRepository repository;

        public JournalSearcher(JournalRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<WorkingHours> searchWindow)
        {
            var periodJournals = GetJournalsApplyingTo(searchWindow.Period).ToList();

            return periodJournals.Where(x => Matches(x, searchWindow.Parameters));
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<UnusualAccountsParameters> searchWindow)
        {
            var periodJournals = GetJournalsApplyingTo(searchWindow.Period).ToList();
            var lookup = new AccountsLookup(periodJournals);
            return lookup.JournalsMadeToUnusualAccountCodes(searchWindow.Parameters.MinimumEntriesToBeConsideredNormal);
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<YearEndParameters> searchWindow)
        {
            var periodJournals = GetJournalsApplyingTo(searchWindow.Period).ToList();

            var startOfSearchPeriod = searchWindow.CreationStartDate();
            return periodJournals.Where(x => x.Created >= startOfSearchPeriod);            
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<UserParameters> searchWindow)
        {
            throw new NotSupportedException("Xero does not record who raises individual journals");
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<KeywordParameters> searchWindow)
        {
            throw new NotSupportedException("Xero does not have the concept of descriptions");
        }

        public IEnumerable<Journal> FindJournalsWithin(SearchWindow<EndingParameters> searchWindow)
        {
            var periodJournals = GetJournalsApplyingTo(searchWindow.Period).ToList();
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
            var pence = amount*100;
            return pence !=0 && pence%magnitude == 0;
        }

        private static bool Matches(Journal x, WorkingHours workingHours)
        {
            return !workingHours.Contains(x.Created);
        }

        private IEnumerable<Journal> GetJournalsApplyingTo(DateRange period)
        {
            return repository.Journals.ToList().Where(x => period.Contains(x.JournalDate));
        }
    }
}
