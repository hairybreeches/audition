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
        private static readonly TimeFrame Weekend = new TimeFrame(DayOfWeek.Saturday, DayOfWeek.Sunday, new LocalTime(0, 0),
            new LocalTime(11, 59));
        
        
        [Test]
        public void SearcherReturnsJournalsPostedOnADayInRange()
        {
            var sundayJournal = GetJournalPostedOn(DayOfWeek.Sunday);
            var searcher = GetJournalSearcher(sundayJournal);

            var weekendJournalIds =
                searcher.FindJournalsWithin(Weekend).Select(x=>x.Id);

            CollectionAssert.AreEqual(weekendJournalIds, new[]{sundayJournal.Id});
        } 
        
        
        [Test]
        public void SearcherDoesNotReturnsJournalsPostedOutsideRange()
        {
            var mondayJournal = GetJournalPostedOn(DayOfWeek.Monday);
            var searcher = GetJournalSearcher(mondayJournal);

            var weekendJournalIds =
                searcher.FindJournalsWithin(Weekend).Select(x=>x.Id);

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
