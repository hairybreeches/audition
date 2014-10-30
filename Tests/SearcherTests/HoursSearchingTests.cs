using System;
using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.SearchWindows;
using Model.Time;
using NodaTime;
using NUnit.Framework;
using Persistence;
using Searching;
using Tests.Mocks;

namespace Tests.SearcherTests
{
    public class HoursSearchingTests
    {
        [TestCase(DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Sunday)]
        [TestCase(DayOfWeek.Sunday, DayOfWeek.Saturday, DayOfWeek.Sunday)]
        [TestCase(DayOfWeek.Saturday, DayOfWeek.Friday, DayOfWeek.Monday)]
        public void SearcherDoesNotReturnJournalsPostedOnADayInRangeUnlessTheTimeMakesThemInteresting(DayOfWeek dayOfWeek, DayOfWeek fromDay, DayOfWeek toDay)
        {
            var journal = PostedOn(dayOfWeek);
            var searcher = CreateSearcher(journal);

            var journalIds =
                searcher.FindJournalsWithin(CreateSearchWindow(fromDay, toDay))
                    .Select(x => x.Id);

            
            CollectionAssert.IsEmpty(journalIds);
        }        

        [TestCase(DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Friday)]
        [TestCase(DayOfWeek.Wednesday, DayOfWeek.Sunday, DayOfWeek.Tuesday)]
        public void SearcherReturnsJournalsPostedOutsideRangeEvenIfTheTimeIsNotInteresting(DayOfWeek dayOfWeek, DayOfWeek fromDay,
            DayOfWeek toDay)
        {
            var journal = PostedOn(dayOfWeek);
            var searcher = CreateSearcher(journal);

            var journalIds =
                searcher.FindJournalsWithin(CreateSearchWindow(fromDay, toDay))
                    .Select(x => x.Id);

            CollectionAssert.AreEqual(new[] { journal.Id }, journalIds.ToList());
        }

        [TestCaseSource("TimesInsideRange")]
        public void SearcherDoesNotReturnJournalsPostedInsideTimeUnlessTheDayMakesThemInteresting(LocalTime journalTime, LocalTime fromTime, LocalTime toTime)
        {
            var journal = PostedAt(journalTime);
            var searcher = CreateSearcher(journal);

            var journalIds =
                searcher.FindJournalsWithin(CreateSearchWindow(fromTime, toTime))
                    .Select(x => x.Id);

            CollectionAssert.IsEmpty(journalIds.ToList());
        }                
        
        [TestCaseSource("TimesOutsideRange")]
        public void SearcherReturnsJournalsPostedOutsideTimeEvenWhenTheDayIsNotInteresting(LocalTime journalTime, LocalTime fromTime, LocalTime toTime)
        {
            var journal = PostedAt(journalTime);
            var searcher = CreateSearcher(journal);

            var journalIds =
                searcher.FindJournalsWithin(CreateSearchWindow(fromTime, toTime))
                    .Select(x => x.Id);
            CollectionAssert.AreEqual(new[] { journal.Id }, journalIds.ToList());            
        }

        [Test]
        public void SearcherDoesNotReturnJournalsPostedAfterFinancialPeriod()
        {
            var journal = Affecting(new DateTime(1991,1,1));
            var searcher = CreateSearcher(journal);

            var journalIds =
                searcher.FindJournalsWithin(CreateSearchWindow(new DateTime(1990,1,1), new DateTime(1990,12,31,23,59,59)))
                    .Select(x => x.Id);

            CollectionAssert.IsEmpty(journalIds);
        }  
        
        [Test]
        public void SearcherDoesNotReturnJournalsPostedBeforeFinancialPeriod()
        {
            var journal = Affecting(new DateTime(1989,12,31,23,59,59));
            var searcher = CreateSearcher(journal);

            var journalIds =
                searcher.FindJournalsWithin(CreateSearchWindow(new DateTime(1990,1,1), new DateTime(1990,12,31,23,59,59)))
                    .Select(x => x.Id);

            CollectionAssert.IsEmpty(journalIds);
        }

        [Test]
        public void SearcherUsesDatesRatherThanDateTimesToDetermineFinancialPeriodAtEnd()
        {
            //given a journal on the last day of the financial period
            var journal = Affecting(new DateTime(1990, 12, 31, 23, 59, 59));
            var searcher = CreateSearcher(journal);

            //and a period created just with the date, rather than the full datetime
            var journalIds =
                searcher.FindJournalsWithin(CreateSearchWindow(new DateTime(1990, 1, 1), new DateTime(1990, 12, 31)))
                    .Select(x => x.Id);

            //the journal should still be defined as being within the financial period
            CollectionAssert.AreEqual(new[] { journal.Id }, journalIds.ToList());
        }       
        
        [Test]
        public void SearcherUsesDatesRatherThanDateTimesToDetermineFinancialPeriodAtStart()
        {
            //given a journal on the first day of the financial period
            var journal = Affecting(new DateTime(1990, 1, 1, 0, 0, 0));
            var searcher = CreateSearcher(journal);

            //and a period created badly with a time on the first date
            var journalIds =
                searcher.FindJournalsWithin(CreateSearchWindow(new DateTime(1990, 1, 1,23,59,59), new DateTime(1990, 12, 31)))
                    .Select(x => x.Id);

            //the journal should still be defined as being within the financial period
            CollectionAssert.AreEqual(new[] { journal.Id }, journalIds.ToList());
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

        private static SearchWindow<WorkingHoursParameters> CreateSearchWindow(DayOfWeek fromDay, DayOfWeek toDay)
        {
            //the journal will never be outside the time, so will be returned iff the day of the week is interesting
            return CreateSearchWindow(new WorkingHoursParameters(fromDay, toDay, new LocalTime(0, 0), new LocalTime(0, 0)));
        }

        private static SearchWindow<WorkingHoursParameters> CreateSearchWindow(LocalTime fromTime, LocalTime toTime)
        {
            //the journal will never be outside the days of the week, so will be returned iff the time is interesting
            return CreateSearchWindow(new WorkingHoursParameters(DayOfWeek.Sunday, DayOfWeek.Saturday, fromTime, toTime));
        }

        private static SearchWindow<WorkingHoursParameters> CreateSearchWindow(DateTime periodStart, DateTime periodEnd)
        {
            //the journal will always be outside the timeframe, so will be returned precisely when it's in the period
            return new SearchWindow<WorkingHoursParameters>(new WorkingHoursParameters(DayOfWeek.Saturday, DayOfWeek.Sunday, new LocalTime(0, 0), new LocalTime(0, 0)), new DateRange(periodStart, periodEnd));
        }

        private static SearchWindow<WorkingHoursParameters> CreateSearchWindow(WorkingHoursParameters workingHours)
        {
            return new SearchWindow<WorkingHoursParameters>(workingHours, new DateRange(new DateTime(1, 1, 1), new DateTime(3000, 12, 31)));
        }

        private static WorkingHoursSearcher CreateSearcher(params Journal[] journals)
        {
            return new WorkingHoursSearcher(new JournalRepository(journals));
        }

        private static Journal PostedOn(DayOfWeek day)
        {
            var dayOfMonth = 6 + (int) day; //the 6th of July 2014 was a Sunday, Sunday is the 0th element of the enum.
            var journal = new Journal(Guid.NewGuid(), 
                new DateTime(2014, 7, dayOfMonth),
                new DateTime(), Enumerable.Empty<JournalLine>());

            Assert.AreEqual(day, journal.Created.DayOfWeek,
                "PostedOn should return a journal posted on the right day of the week");
            
            return journal;
        }

        private static Journal PostedAt(LocalTime journalTime)
        {
            return new Journal(Guid.NewGuid(),
                new DateTime(2014, 7, 23, journalTime.Hour, journalTime.Minute, journalTime.Second),
                new DateTime(2012,1,3), Enumerable.Empty<JournalLine>());
        }

        private static Journal Affecting(DateTime dateTime)
        {
            return new Journal(Guid.NewGuid(),
                new DateTime(2014, 7, 1),
                dateTime, Enumerable.Empty<JournalLine>());
        }
    }
}
