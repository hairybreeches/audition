using System;
using System.Linq;
using Model.Accounting;
using Model.SearchWindows;
using Model.Time;
using NUnit.Framework;
using Tests.Mocks;

namespace Tests.SearcherTests
{
    [TestFixture]
    public class XeroRoundNumberSearchingTests
    {
        private static readonly DateTime YearEnd = new DateTime(2012, 3, 31);
        private static readonly DateTime YearStart = YearEnd.Subtract(TimeSpan.FromDays(365));
        private static readonly DateTime InPeriod = YearEnd.Subtract(TimeSpan.FromDays(30));
        private static readonly DateRange FinancialPeriod = new DateRange(YearStart, YearEnd);

        [Test]
        public void DoesNotReturnJournalsWhichDoNotApplyToTheFinancialPeriod()
        {
            var journalApplyingToPostYearEnd = Mock.JournalForAmount(YearEnd.Subtract(TimeSpan.FromDays(2)), YearEnd.AddDays(1), 1000);
            var journalApplyingToPreYearstart = Mock.JournalForAmount(YearEnd.Subtract(TimeSpan.FromDays(2)), YearStart.Subtract(TimeSpan.FromDays(1)), 1000);

            var searcher = Mock.JournalSearcher(journalApplyingToPostYearEnd, journalApplyingToPreYearstart);
            var result = searcher.FindJournalsWithin(new SearchWindow<EndingParameters>(new EndingParameters(1),FinancialPeriod ));
            CollectionAssert.IsEmpty(result);
        }     
        
        
        [Test]
        public void DoesNotReturnJournalsWithALineOfZeroValue()
        {
            var journalForZero = Mock.JournalForAmount(InPeriod, InPeriod, 0);            
            var searcher = Mock.JournalSearcher(journalForZero);
            var result = searcher.FindJournalsWithin(new SearchWindow<EndingParameters>(new EndingParameters(1),FinancialPeriod ));
            CollectionAssert.IsEmpty(result);
        }  
        
        
        [Test]
        public void ReturnsJournalForRoundAmount()
        {
            var journalForRoundAmount = Mock.JournalForAmount(InPeriod, InPeriod, 1000);            
            var searcher = Mock.JournalSearcher(journalForRoundAmount);
            var result = searcher.FindJournalsWithin(new SearchWindow<EndingParameters>(new EndingParameters(1),FinancialPeriod ));
            CollectionAssert.AreEquivalent(new[]{journalForRoundAmount}, result);
        }        
        
        [Test]
        public void ReturnsJournalWithExactlyTheRightAmountOfZeroes()
        {
            var journalForRoundAmount = Mock.JournalForAmount(InPeriod, InPeriod, 1000);            
            var searcher = Mock.JournalSearcher(journalForRoundAmount);
            var result = searcher.FindJournalsWithin(new SearchWindow<EndingParameters>(new EndingParameters(3),FinancialPeriod ));
            CollectionAssert.AreEquivalent(new[]{journalForRoundAmount}, result);
        }    
        
        
        [Test]
        public void DoesNotReturnJournalWithOneTooFewZeroes()
        {
            var journalForRoundAmount = Mock.JournalForAmount(InPeriod, InPeriod, 10000);            
            var searcher = Mock.JournalSearcher(journalForRoundAmount);
            var result = searcher.FindJournalsWithin(new SearchWindow<EndingParameters>(new EndingParameters(5),FinancialPeriod ));
            CollectionAssert.IsEmpty(result);
        }
    }
}
