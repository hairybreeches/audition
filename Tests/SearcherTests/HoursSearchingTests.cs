using System;
using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;
using NodaTime;
using NUnit.Framework;
using Searching;
using Searching.SearchWindows;

namespace Tests.SearcherTests
{
    public class HoursSearchingTests
    {
        [TestCase(DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Sunday)]
        [TestCase(DayOfWeek.Sunday, DayOfWeek.Saturday, DayOfWeek.Sunday)]
        [TestCase(DayOfWeek.Saturday, DayOfWeek.Friday, DayOfWeek.Monday)]
        public void SearcherDoesNotReturnTransactionsPostedOnADayInRangeUnlessTheTimeMakesThemInteresting(DayOfWeek dayOfWeek, DayOfWeek fromDay, DayOfWeek toDay)
        {
            var transaction = PostedOn(dayOfWeek);


            var transactionIds = Searching.ExecuteSearch(CreateSearchWindow(fromDay, toDay), transaction)
                .Select(x => x.Id);

            
            CollectionAssert.IsEmpty(transactionIds);
        }        

        [TestCase(DayOfWeek.Sunday, DayOfWeek.Monday, DayOfWeek.Friday)]
        [TestCase(DayOfWeek.Wednesday, DayOfWeek.Sunday, DayOfWeek.Tuesday)]
        public void SearcherReturnsTransactionsPostedOutsideRangeEvenIfTheTimeIsNotInteresting(DayOfWeek dayOfWeek, DayOfWeek fromDay,
            DayOfWeek toDay)
        {
            var transaction = PostedOn(dayOfWeek);

            var transactionIds = Searching.ExecuteSearch(CreateSearchWindow(fromDay, toDay), transaction)
                    .Select(x => x.Id);

            CollectionAssert.AreEqual(new[] { transaction.Id }, transactionIds.ToList());
        }

        [TestCaseSource("TimesInsideRange")]
        public void SearcherDoesNotReturnTransactionsPostedInsideTimeUnlessTheDayMakesThemInteresting(LocalTime transactionTime, LocalTime fromTime, LocalTime toTime)
        {
            var transaction = PostedAt(transactionTime);

            var transactionIds = Searching.ExecuteSearch(CreateSearchWindow(fromTime, toTime), transaction)
                    .Select(x => x.Id);

            CollectionAssert.IsEmpty(transactionIds.ToList());
        }                
        
        [TestCaseSource("TimesOutsideRange")]
        public void SearcherReturnsTransactionsPostedOutsideTimeEvenWhenTheDayIsNotInteresting(LocalTime transactionTime, LocalTime fromTime, LocalTime toTime)
        {
            var transaction = PostedAt(transactionTime);
            var transactionIds = Searching.ExecuteSearch(CreateSearchWindow(fromTime, toTime), transaction)
                    .Select(x => x.Id);

            CollectionAssert.AreEqual(new[] { transaction.Id }, transactionIds.ToList());            
        }

        [Test]
        public void SearcherDoesNotReturnTransactionsPostedAfterFinancialPeriod()
        {
            var transaction = Affecting(new DateTime(1991,1,1));
            var transactionIds = Searching.ExecuteSearch(CreateSearchWindow(new DateTime(1990,1,1), new DateTime(1990,12,31,23,59,59)), transaction)
                    .Select(x => x.Id);

            CollectionAssert.IsEmpty(transactionIds);
        }  
        
        [Test]
        public void SearcherDoesNotReturnTransactionsPostedBeforeFinancialPeriod()
        {
            var transaction = Affecting(new DateTime(1989,12,31,23,59,59));

            var transactionIds = Searching.ExecuteSearch(CreateSearchWindow(new DateTime(1990,1,1), new DateTime(1990,12,31,23,59,59)), transaction)
                    .Select(x => x.Id);

            CollectionAssert.IsEmpty(transactionIds);
        }

        [Test]
        public void SearcherUsesDatesRatherThanDateTimesToDetermineFinancialPeriodAtEnd()
        {
            //given a transaction on the last day of the financial period
            var transaction = Affecting(new DateTime(1990, 12, 31, 23, 59, 59));

            //and a period created just with the date, rather than the full datetime
            var transactionIds =Searching.ExecuteSearch(CreateSearchWindow(new DateTime(1990, 1, 1), new DateTime(1990, 12, 31)), transaction)
                    .Select(x => x.Id);

            //the transaction should still be defined as being within the financial period
            CollectionAssert.AreEqual(new[] { transaction.Id }, transactionIds.ToList());
        }       
        
        [Test]
        public void SearcherUsesDatesRatherThanDateTimesToDetermineFinancialPeriodAtStart()
        {
            //given a transaction on the first day of the financial period
            var transaction = Affecting(new DateTime(1990, 1, 1, 0, 0, 0));

            //and a period created badly with a time on the first date
            var transactionIds = Searching.ExecuteSearch(CreateSearchWindow(new DateTime(1990, 1, 1,23,59,59), new DateTime(1990, 12, 31)), transaction)
                    .Select(x => x.Id);

            //the transaction should still be defined as being within the financial period
            CollectionAssert.AreEqual(new[] { transaction.Id }, transactionIds.ToList());
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
            //the transaction will never be outside the time, so will be returned iff the day of the week is interesting
            return CreateSearchWindow(new WorkingHoursParameters(fromDay, toDay, new LocalTime(0, 0), new LocalTime(0, 0)));
        }

        private static SearchWindow<WorkingHoursParameters> CreateSearchWindow(LocalTime fromTime, LocalTime toTime)
        {
            //the transaction will never be outside the days of the week, so will be returned iff the time is interesting
            return CreateSearchWindow(new WorkingHoursParameters(DayOfWeek.Sunday, DayOfWeek.Saturday, fromTime, toTime));
        }

        private static SearchWindow<WorkingHoursParameters> CreateSearchWindow(DateTime periodStart, DateTime periodEnd)
        {
            //the transaction will always be outside the timeframe, so will be returned precisely when it's in the period
            return new SearchWindow<WorkingHoursParameters>(new WorkingHoursParameters(DayOfWeek.Saturday, DayOfWeek.Sunday, new LocalTime(0, 0), new LocalTime(0, 0)), new DateRange(periodStart, periodEnd));
        }

        private static SearchWindow<WorkingHoursParameters> CreateSearchWindow(WorkingHoursParameters workingHours)
        {
            return new SearchWindow<WorkingHoursParameters>(workingHours, new DateRange(new DateTime(1, 1, 1), new DateTime(3000, 12, 31)));
        }

        private static Transaction PostedOn(DayOfWeek day)
        {
            var dayOfMonth = 6 + (int) day; //the 6th of July 2014 was a Sunday, Sunday is the 0th element of the enum.
            var transaction = new Transaction(Guid.NewGuid(), 
                new DateTime(2014, 7, dayOfMonth),
                new DateTime(), Enumerable.Empty<LedgerEntry>());

            Assert.AreEqual(day, transaction.Created.DayOfWeek,
                "PostedOn should return a transaction posted on the right day of the week");
            
            return transaction;
        }

        private static Transaction PostedAt(LocalTime transactionTime)
        {
            return new Transaction(Guid.NewGuid(),
                new DateTime(2014, 7, 23, transactionTime.Hour, transactionTime.Minute, transactionTime.Second),
                new DateTime(2012,1,3), Enumerable.Empty<LedgerEntry>());
        }

        private static Transaction Affecting(DateTime dateTime)
        {
            return new Transaction(Guid.NewGuid(),
                new DateTime(2014, 7, 1),
                dateTime, Enumerable.Empty<LedgerEntry>());
        }
    }
}
