using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using NodaTime;
using NSubstitute;
using NUnit.Framework;
using Xero;
using XeroApi.Model;
using Journal = Model.Journal;


namespace Tests
{
    public class XeroJournalSearcherTests
    {
        [TestCase(DayOfWeek.Monday, DayOfWeek.Monday, DayOfWeek.Sunday)]
        [TestCase(DayOfWeek.Sunday, DayOfWeek.Saturday, DayOfWeek.Sunday)]
        public void SearcherReturnsJournalsPostedOnADayInRange(DayOfWeek dayOfWeek, DayOfWeek fromDay, DayOfWeek toDay)
        {
            var journal = GetJournalPostedOn(dayOfWeek);
            var searcher = GetJournalSearcher(journal);

            var allJournalIds =
                searcher.FindJournalsWithin(new TimeFrame(fromDay, toDay, new LocalTime(0, 0),new LocalTime(11, 59)))
                .Select(x => x.Id);

            CollectionAssert.AreEqual(new[] {journal.Id}, allJournalIds.ToList());
        }

        [TestCase(DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Friday)]
        public void SearcherDoesNotReturnsJournalsPostedOutsideRange(DayOfWeek dayOfWeek, DayOfWeek fromDay, DayOfWeek toDay)
        {
            var sundayJournal = GetJournalPostedOn(dayOfWeek);
            var searcher = GetJournalSearcher(sundayJournal);

            var weekendJournalIds =
                searcher.FindJournalsWithin(new TimeFrame(fromDay, toDay, new LocalTime(0, 0), new LocalTime(11, 59))).Select(x => x.Id);

            CollectionAssert.IsEmpty(weekendJournalIds);
        }

        private IJournalSearcher GetJournalSearcher(params Journal[] journals)
        {
            var repository = Substitute.For<IFullRepository>();
            repository.Journals.Returns(journals.Select(x => x.ToXeroJournal()).AsQueryable());
            var factory = Substitute.For<IRepositoryFactory>();
            factory.CreateRepository().Returns(repository);
            return new XeroJournalSearcher(factory);
        }

        private Journal GetJournalPostedOn(DayOfWeek day)
        {
            var dayOfMonth = 6 + (int) day; //the 6th of July 2014 was a Sunday, Sunday is the 0th element of the enum.
            var journal = new Journal(Guid.NewGuid(), 
                new DateTime(2014, 7, dayOfMonth),
                new DateTime());

            Assert.AreEqual(day, journal.Created.DayOfWeek,
                "GetJournalPostedOn should return a journal posted on the right day of the week");
            
            return journal;
        }


    }
}
