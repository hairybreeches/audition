using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Accounting;
using Model.SearchWindows;
using Model.Time;
using NUnit.Framework;
using Tests.Mocks;

namespace Tests.SearcherTests
{
    [TestFixture]
    public class XeroDateSearchingTests
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
            var searcher = Mock.JournalSearcher(postYearEndJournal);            
            var result = searcher.FindJournalsWithin(SearchParameters);
            CollectionAssert.AreEquivalent(new []{postYearEndJournal}, result);
        }
        
        [Test]
        public void ReturnsJournalsPostedNearYearEnd()
        {            
            var nearYearEndJournal = new Journal(Guid.NewGuid(), YearEnd.Subtract(TimeSpan.FromDays(2)), YearEnd.Subtract(TimeSpan.FromDays(60)),Enumerable.Empty<JournalLine>() );
            var searcher = Mock.JournalSearcher(nearYearEndJournal);            
            var result = searcher.FindJournalsWithin(SearchParameters);
            CollectionAssert.AreEquivalent(new []{nearYearEndJournal}, result);
        }
        
        [Test]
        public void ReturnsJournalsPostedExactlyNumberOfDaysBeforeYearEnd()
        {            
            var nearYearEndJournal = new Journal(Guid.NewGuid(), YearEnd.Subtract(TimeSpan.FromDays(7)), YearEnd.Subtract(TimeSpan.FromDays(60)),Enumerable.Empty<JournalLine>() );
            var searcher = Mock.JournalSearcher(nearYearEndJournal);            
            var result = searcher.FindJournalsWithin(new SearchWindow<YearEndParameters>(new YearEndParameters(7),FinancialPeriod ));
            CollectionAssert.AreEquivalent(new []{nearYearEndJournal}, result);
        }

        [Test]
        public void DoesNotReturnJournalsWhichDoNotApplyToTheFinancialPeriod()
        {
            var journalApplyingToPostYearEnd = new Journal(Guid.NewGuid(), YearEnd.Subtract(TimeSpan.FromDays(2)), YearEnd.AddDays(1), Enumerable.Empty<JournalLine>());
            var journalApplyingToPreYearstart = new Journal(Guid.NewGuid(), YearEnd.Subtract(TimeSpan.FromDays(2)), YearStart.Subtract(TimeSpan.FromDays(1)), Enumerable.Empty<JournalLine>());

            var searcher = Mock.JournalSearcher(journalApplyingToPostYearEnd, journalApplyingToPreYearstart);
            var result = searcher.FindJournalsWithin(SearchParameters);
            CollectionAssert.IsEmpty(result);
        }
        
        [Test]
        public void DoesNotReturnJournalsWhichAreInThePeriodAndNotCloseToYearEnd()
        {
            var journalNotNearEnoughToYearEnd = new Journal(Guid.NewGuid(), YearEnd.Subtract(TimeSpan.FromDays(6)), YearEnd.Subtract(TimeSpan.FromDays(60)), Enumerable.Empty<JournalLine>());

            var searcher = Mock.JournalSearcher(journalNotNearEnoughToYearEnd);
            var result = searcher.FindJournalsWithin(SearchParameters);
            CollectionAssert.IsEmpty(result);
        }
    }
}
