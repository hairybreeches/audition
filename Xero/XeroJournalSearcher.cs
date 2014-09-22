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
            var periodJournals = GetJournals(searchWindow.Period).ToList();

            return periodJournals.Where(x => Matches(searchWindow, x)).Select(x => x.ToModelJournal());
        }

        public IEnumerable<Model.Accounting.Journal> FindJournalsWithin(SearchWindow<UnusualAccountsParameters> searchWindow)
        {
            var periodJournals = GetJournals(searchWindow.Period).ToList();
            var lookup = new AccountsLookup(periodJournals);
            return lookup.JournalsMadeToUnusualAccountCodes(searchWindow.Parameters.Quantity)
                .Select(XeroJournalExtensions.ToModelJournal);
        }

        public static bool Matches(SearchWindow<WorkingHours> searchWindow, Journal x)
        {
            return !searchWindow.Parameters.Contains(x.CreatedDateUTC);
        }

        private IEnumerable<Journal> GetJournals(DateRange period)
        {
            return repository.Journals.ToList().Where(x => period.Contains(x.JournalDate));
        }
    }
}
