﻿using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using NodaTime;
using NSubstitute;
using NUnit.Framework;
using Xero;
using Journal = Model.Journal;
using Period = Model.Period;


namespace Tests
{
    public class XeroJournalSearcherTests
    {
        [TestCase(DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Sunday)]
        [TestCase(DayOfWeek.Sunday, DayOfWeek.Saturday, DayOfWeek.Sunday)]
        [TestCase(DayOfWeek.Saturday, DayOfWeek.Friday, DayOfWeek.Monday)]
        public void SearcherDoesNotReturnJournalsPostedOnADayInRange(DayOfWeek dayOfWeek, DayOfWeek fromDay, DayOfWeek toDay)
        {
            var journal = GetJournalPostedOn(dayOfWeek);
            var searcher = GetJournalSearcher(journal);

            var journalIds =
                searcher.FindJournalsWithin(CreateSearchWindow(fromDay, toDay))
                    .Select(x => x.Id);

            
            CollectionAssert.IsEmpty(journalIds);
        }        

        [TestCase(DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Friday)]
        [TestCase(DayOfWeek.Wednesday, DayOfWeek.Sunday, DayOfWeek.Tuesday)]
        public void SearcherReturnsJournalsPostedOutsideRange(DayOfWeek dayOfWeek, DayOfWeek fromDay,
            DayOfWeek toDay)
        {
            var journal = GetJournalPostedOn(dayOfWeek);
            var searcher = GetJournalSearcher(journal);

            var journalIds =
                searcher.FindJournalsWithin(CreateSearchWindow(fromDay, toDay))
                    .Select(x => x.Id);

            CollectionAssert.AreEqual(new[] { journal.Id }, journalIds.ToList());
        }

        [TestCaseSource("TimesInsideRange")]
        public void SearcherDoesNotReturnJournalsPostedInsideTime(LocalTime journalTime, LocalTime fromTime, LocalTime toTime)
        {
            var journal = GetJournalPostedAt(journalTime);
            var searcher = GetJournalSearcher(journal);

            var journalIds =
                searcher.FindJournalsWithin(CreateSearchWindow(fromTime, toTime))
                    .Select(x => x.Id);

            CollectionAssert.IsEmpty(journalIds.ToList());
        }                
        
        [TestCaseSource("TimesOutsideRange")]
        public void SearcherReturnsJournalsPostedOutsideTime(LocalTime journalTime, LocalTime fromTime, LocalTime toTime)
        {
            var journal = GetJournalPostedAt(journalTime);
            var searcher = GetJournalSearcher(journal);

            var journalIds =
                searcher.FindJournalsWithin(CreateSearchWindow(fromTime, toTime))
                    .Select(x => x.Id);
            CollectionAssert.AreEqual(new[] { journal.Id }, journalIds.ToList());            
        }

        [Test]
        public void CannotCreateATimeFrameWithTimesWhichWrapAround()
        {
            Assert.Throws<InvalidTimeFrameException>(
                () => new TimeFrame(DayOfWeek.Monday, DayOfWeek.Saturday, new LocalTime(16, 0), new LocalTime(15, 0)));
        }

        IEnumerable<TestCaseData> TimesInsideRange
        {
            get
            {
                yield return new TestCaseData(new LocalTime(15, 0), new LocalTime(12, 0), new LocalTime(17, 0));
                yield return new TestCaseData(new LocalTime(11, 30), new LocalTime(11, 0), new LocalTime(11, 30));
                yield return new TestCaseData(new LocalTime(10, 46), new LocalTime(10, 46), new LocalTime(11, 30));
            }
        }

        IEnumerable<TestCaseData> TimesOutsideRange
        {
            get { 
                yield return new TestCaseData(new LocalTime(19, 0), new LocalTime(15, 0), new LocalTime(17, 0)); 
                yield return new TestCaseData(new LocalTime(9, 0), new LocalTime(15, 0), new LocalTime(17, 0));  
                yield return new TestCaseData(new LocalTime(8, 47), new LocalTime(8, 48), new LocalTime(17, 0));  
                yield return new TestCaseData(new LocalTime(17, 14), new LocalTime(8, 48), new LocalTime(17, 13));  
            }
        }

        private Journal GetJournalPostedAt(LocalTime journalTime)
        {
            return new Journal(Guid.NewGuid(),
                new DateTime(2014, 7, 23, journalTime.Hour, journalTime.Minute, journalTime.Second),
                new DateTime(), Enumerable.Empty<JournalLine>());
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
                new DateTime(), Enumerable.Empty<JournalLine>());

            Assert.AreEqual(day, journal.Created.DayOfWeek,
                "GetJournalPostedOn should return a journal posted on the right day of the week");
            
            return journal;
        }

        private static SearchWindow CreateSearchWindow(DayOfWeek fromDay, DayOfWeek toDay)
        {
            //the journal will never be outside the time, so will be returned iff the day of the week is interesting
            return CreateSearchWindow(new TimeFrame(fromDay, toDay, new LocalTime(0, 0), new LocalTime(0, 0)));
        }

        private static SearchWindow CreateSearchWindow(LocalTime fromTime, LocalTime toTime)
        {
            //the journal will never be outside the days of the week, so will be returned iff the time is interesting
            return CreateSearchWindow(new TimeFrame(DayOfWeek.Sunday, DayOfWeek.Saturday, fromTime, toTime));
        }

        private static SearchWindow CreateSearchWindow(TimeFrame timeFrame)
        {
            return new SearchWindow(timeFrame, new Period(new DateTime(1,1,1), new DateTime(3000,12,31)));
        }
    }
}
