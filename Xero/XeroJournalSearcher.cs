using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using NodaTime;
using XeroApi.Model;

namespace Xero
{
    public class XeroJournalSearcher : IJournalSearcher
    {
        private readonly IFullRepository repository;

        public XeroJournalSearcher(IRepositoryFactory repositoryFactory)
        {
            repository = repositoryFactory.CreateRepository();
        }

        public IEnumerable<Model.Journal> FindJournalsWithin(SearchWindow searchWindow)
        {
            var allJournals = repository.Journals.ToList();
            return allJournals.Where(x => Within(searchWindow.Outside, x.CreatedDateUTC)).Select(x => x.ToModelJournal());
        }

        private bool Within(TimeFrame timeFrame, DateTime dateTime)
        {
            return DayWithinRange(timeFrame, dateTime) && TimeWithinRange(timeFrame, dateTime);
        }

        private bool TimeWithinRange(TimeFrame timeFrame, DateTime dateTime)
        {
            var createdDateUtc = dateTime;
            var journalCreationTime = new LocalTime(createdDateUtc.Hour,createdDateUtc.Minute, createdDateUtc.Second);

            return journalCreationTime <= timeFrame.ToTime
                   && journalCreationTime >= timeFrame.FromTime;
        }

        private bool DayWithinRange(TimeFrame timeFrame, DateTime date)
        {
            var creationDay = (int) date.DayOfWeek;

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
