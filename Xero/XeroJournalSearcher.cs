using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.SearchWindows;
using NodaTime;
using XeroApi.Model;
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
            var allJournals = repository.Journals.ToList();
            return allJournals.Where(x => Matches(searchWindow, x)).Select(x => x.ToModelJournal());
        }

        public static bool Matches(HoursSearchWindow searchWindow, Journal x)
        {
            return searchWindow.Period.Contains(x.JournalDate) && 
                !searchWindow.Parameters.Contains(x.CreatedDateUTC);
        }
    }
}
