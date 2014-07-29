using System.Collections.Generic;
using System.Linq;
using Model;
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

        public IEnumerable<Model.Journal> FindJournalsWithin(TimeFrame timeFrame)
        {
            var allJournals = repository.Journals.ToList();
            return allJournals.Where(x => DayWithinRange(x, timeFrame) && TimeWithinRange(x, timeFrame)).Select(x => x.ToModelJournal());
        }

        private bool TimeWithinRange(Journal journal, TimeFrame timeFrame)
        {
            var createdDateUtc = journal.CreatedDateUTC;
            var journalCreationTime = new LocalTime(createdDateUtc.Hour,createdDateUtc.Minute, createdDateUtc.Second);

            return journalCreationTime <= timeFrame.ToTime
                   && journalCreationTime >= timeFrame.FromTime;
        }

        private bool DayWithinRange(Journal journal, TimeFrame timeFrame)
        {
            var creationDay = (int) journal.CreatedDateUTC.DayOfWeek;

            var fromDay = (int) timeFrame.FromDay;
            var toDay = (int) timeFrame.ToDay;

            if (toDay <= fromDay)
            {
                toDay += 7;
            }

            if (creationDay < fromDay)
            {
                creationDay += 7;
            }


            return creationDay <= toDay;
        }
    }
}
