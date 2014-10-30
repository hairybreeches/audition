using System;
using System.Linq;
using Model.Accounting;
using Model.Persistence;
using Model.Searching;
using Model.SearchWindows;
using Model.Time;
using NUnit.Framework;

namespace Tests.SearcherTests
{
    [TestFixture]
    public class YearEndSearchingTests
    {
        private static readonly DateTime YearEnd = new DateTime(2012, 3, 31);
        private static readonly DateTime YearStart = YearEnd.Subtract(TimeSpan.FromDays(365));
        private static readonly DateRange FinancialPeriod = new DateRange(YearStart, YearEnd);

        private static readonly SearchWindow<YearEndParameters> SearchParameters = new SearchWindow<YearEndParameters>(new YearEndParameters(5),
                FinancialPeriod);

        

        [Test]
        public void ReturnsJournalsPostedAfterYearEnd()
        {            
            var postYearEndJournal = new Journal(Guid.NewGuid(), YearEnd.AddDays(1), YearEnd.Subtract(TimeSpan.FromDays(60)),Enumerable.Empty<JournalLine>() );
            var searcher = CreateSearcher(postYearEndJournal);            
            var result = searcher.FindJournalsWithin(SearchParameters);
            CollectionAssert.AreEquivalent(new []{postYearEndJournal}, result);
        }
        
        [Test]
        public void ReturnsJournalsPostedNearYearEnd()
        {            
            var nearYearEndJournal = new Journal(Guid.NewGuid(), YearEnd.Subtract(TimeSpan.FromDays(2)), YearEnd.Subtract(TimeSpan.FromDays(60)),Enumerable.Empty<JournalLine>() );
            var searcher = CreateSearcher(nearYearEndJournal);            
            var result = searcher.FindJournalsWithin(SearchParameters);
            CollectionAssert.AreEquivalent(new []{nearYearEndJournal}, result);
        }
        
        [Test]
        public void ReturnsJournalsPostedExactlyNumberOfDaysBeforeYearEnd()
        {            
            var nearYearEndJournal = new Journal(Guid.NewGuid(), YearEnd.Subtract(TimeSpan.FromDays(7)), YearEnd.Subtract(TimeSpan.FromDays(60)),Enumerable.Empty<JournalLine>() );
            var searcher = CreateSearcher(nearYearEndJournal);            
            var result = searcher.FindJournalsWithin(new SearchWindow<YearEndParameters>(new YearEndParameters(7),FinancialPeriod ));
            CollectionAssert.AreEquivalent(new []{nearYearEndJournal}, result);
        }

        [Test]
        public void DoesNotReturnJournalsWhichDoNotApplyToTheFinancialPeriod()
        {
            var timeInsideTheFinancialPeriod = YearEnd.Subtract(TimeSpan.FromDays(2));
            var journalApplyingToPostYearEnd = new Journal(Guid.NewGuid(), timeInsideTheFinancialPeriod, YearEnd.AddDays(1), Enumerable.Empty<JournalLine>());
            var journalApplyingToPreYearstart = new Journal(Guid.NewGuid(), timeInsideTheFinancialPeriod, YearStart.Subtract(TimeSpan.FromDays(1)), Enumerable.Empty<JournalLine>());

            var searcher = CreateSearcher(journalApplyingToPostYearEnd, journalApplyingToPreYearstart);
            var result = searcher.FindJournalsWithin(SearchParameters);
            CollectionAssert.IsEmpty(result);
        }
        
        [Test]
        public void DoesNotReturnJournalsWhichAreInThePeriodAndNotCloseToYearEnd()
        {
            var journalNotNearEnoughToYearEnd = new Journal(Guid.NewGuid(), YearEnd.Subtract(TimeSpan.FromDays(6)), YearEnd.Subtract(TimeSpan.FromDays(60)), Enumerable.Empty<JournalLine>());

            var searcher = CreateSearcher(journalNotNearEnoughToYearEnd);
            var result = searcher.FindJournalsWithin(SearchParameters);
            CollectionAssert.IsEmpty(result);
        }

        private static YearEndSearcher CreateSearcher(params Journal[] journals)
        {
            return new YearEndSearcher(new JournalRepository(journals));
        }
    }
}
