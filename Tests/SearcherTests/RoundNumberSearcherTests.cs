using System;
using Model.Accounting;
using Model.Time;
using NUnit.Framework;
using Searching;
using Searching.SearchWindows;

namespace Tests.SearcherTests
{
    [TestFixture]
    public class RoundNumberSearcherTests
    {
        private static readonly DateTime YearEnd = new DateTime(2012, 3, 31);
        private static readonly DateTime YearStart = YearEnd.Subtract(TimeSpan.FromDays(365));
        private static readonly DateTime InPeriod = YearEnd.Subtract(TimeSpan.FromDays(30));
        private static readonly DateRange FinancialPeriod = new DateRange(YearStart, YearEnd);

        [Test]
        public void DoesNotReturnJournalsWhichDoNotApplyToTheFinancialPeriod()
        {
            var journalApplyingToPostYearEnd = ForAmount(YearEnd.Subtract(TimeSpan.FromDays(2)), YearEnd.AddDays(1), 1000);
            var journalApplyingToPreYearstart = ForAmount(YearEnd.Subtract(TimeSpan.FromDays(2)), YearStart.Subtract(TimeSpan.FromDays(1)), 1000);

            var result = Searching.ExecuteSearch(new SearchWindow<EndingParameters>(new EndingParameters(1), FinancialPeriod), journalApplyingToPostYearEnd, journalApplyingToPreYearstart);
            CollectionAssert.IsEmpty(result);
        }     
        
        
        [Test]
        public void DoesNotReturnJournalsWithALineOfZeroValue()
        {
            var journalForZero = ForAmount(InPeriod, InPeriod, 0);
            var result = Searching.ExecuteSearch((new SearchWindow<EndingParameters>(new EndingParameters(1),FinancialPeriod )), journalForZero);
            CollectionAssert.IsEmpty(result);
        }  
        
        
        [Test]
        public void ReturnsJournalForRoundAmount()
        {
            var journalForRoundAmount = ForAmount(InPeriod, InPeriod, 1000);
            var result = Searching.ExecuteSearch(new SearchWindow<EndingParameters>(new EndingParameters(1),FinancialPeriod ), journalForRoundAmount);
            CollectionAssert.AreEquivalent(new[]{journalForRoundAmount}, result);
        }        
        
        [Test]
        public void ReturnsJournalWithExactlyTheRightAmountOfZeroes()
        {
            var journalForRoundAmount = ForAmount(InPeriod, InPeriod, 1000);
            var result = Searching.ExecuteSearch(new SearchWindow<EndingParameters>(new EndingParameters(3),FinancialPeriod ), journalForRoundAmount);
            CollectionAssert.AreEquivalent(new[]{journalForRoundAmount}, result);
        }    
        
        
        [Test]
        public void DoesNotReturnJournalWithOneTooFewZeroes()
        {
            var journalForRoundAmount = ForAmount(InPeriod, InPeriod, 10000);
            var result = Searching.ExecuteSearch(new SearchWindow<EndingParameters>(new EndingParameters(5),FinancialPeriod ), journalForRoundAmount);
            CollectionAssert.IsEmpty(result);
        }


        private static Transaction ForAmount(DateTime creationDate, DateTime journalDate, int amountOfPence)
        {
            var amountOfPounds = ((decimal) amountOfPence)/100;
            return new Transaction(Guid.NewGuid(), creationDate, journalDate, new []{ new LedgerEntry("a", "a", JournalType.Cr, amountOfPounds), new LedgerEntry("b", "b", JournalType.Dr, amountOfPounds)});
        }
    }
}
