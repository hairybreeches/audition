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

        public IEnumerable<Model.Accounting.Journal> FindJournalsWithin(HoursSearchWindow searchWindow)
        {
            var periodJournals = GetJournals(searchWindow.Period).ToList();

            return periodJournals.Where(x => Matches(searchWindow, x)).Select(x => x.ToModelJournal());
        }

        public static bool Matches(HoursSearchWindow searchWindow, Journal x)
        {
            return !searchWindow.Parameters.Contains(x.CreatedDateUTC);
        }

        private IEnumerable<Journal> GetJournals(DateRange period)
        {
            repository.Journals.Where(x => period.Contains(x.JournalDate));
        }
    }
}
