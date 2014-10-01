using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.SearchWindows;
using Model.Time;
using Journal = XeroApi.Model.Journal;

namespace Xero
{
    public class XeroJournalSearcher : IJournalSearcher
    {
        private readonly IFullRepository repository;

        public XeroJournalSearcher(IRepositoryFactory repositoryFactory)
        {
            repository = repositoryFactory.CreateRepository();
        }

        public IEnumerable<Model.Accounting.Journal> FindJournalsWithin(SearchWindow<WorkingHours> searchWindow)
        {
            var periodJournals = GetJournalsApplyingTo(searchWindow.Period).ToList();

            return periodJournals.Where(x => Matches(searchWindow, x)).Select(x => x.ToModelJournal());
        }

        public IEnumerable<Model.Accounting.Journal> FindJournalsWithin(SearchWindow<UnusualAccountsParameters> searchWindow)
        {
            var periodJournals = GetJournalsApplyingTo(searchWindow.Period).ToList();
            var lookup = new AccountsLookup(periodJournals);
            return lookup.JournalsMadeToUnusualAccountCodes(searchWindow.Parameters.MinimumEntriesToBeConsideredNormal)
                .Select(XeroJournalExtensions.ToModelJournal);
        }

        public IEnumerable<Model.Accounting.Journal> FindJournalsWithin(SearchWindow<YearEndParameters> searchWindow)
        {
            
            var periodEndDate = searchWindow.Period.To;
            var startOfSearchPeriod =
                periodEndDate.Subtract(TimeSpan.FromDays(searchWindow.Parameters.DaysBeforeYearEnd));
            var dateRange = new DateRange(startOfSearchPeriod, DateTime.MaxValue);
            var journalsPostedInTimeFrame = GetJournalsPostedDuring(dateRange).ToList();
            return journalsPostedInTimeFrame
                .Where(x => searchWindow.Period.Contains(x.CreatedDateUTC))
                .Select(x=>x.ToModelJournal());

        }

        public IEnumerable<Model.Accounting.Journal> FindJournalsWithin(SearchWindow<UserParameters> searchWindow)
        {
            throw new NotSupportedException("Xero does not record who raises individual journals");
        }

        public static bool Matches(SearchWindow<WorkingHours> searchWindow, Journal x)
        {
            return !searchWindow.Parameters.Contains(x.CreatedDateUTC);
        }

        private IEnumerable<Journal> GetJournalsApplyingTo(DateRange period)
        {
            return repository.Journals.ToList().Where(x => period.Contains(x.JournalDate));
        }
        
        private IEnumerable<Journal> GetJournalsPostedDuring(DateRange period)
        {
            return repository.Journals.ToList().Where(x => period.Contains(x.CreatedDateUTC));
        }
    }
}
