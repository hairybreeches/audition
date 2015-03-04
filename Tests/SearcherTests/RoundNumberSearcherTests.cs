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
        public void DoesNotReturnTransactionsWhichDoNotApplyToTheFinancialPeriod()
        {
            var transactionApplyingToPostYearEnd = ForAmount(YearEnd.AddDays(1), 1000);
            var transactionApplyingToPreYearstart = ForAmount(YearStart.Subtract(TimeSpan.FromDays(1)), 1000);

            var result = Searching.ExecuteSearch(new SearchWindow<EndingParameters>(new EndingParameters(1), FinancialPeriod), transactionApplyingToPostYearEnd, transactionApplyingToPreYearstart);
            CollectionAssert.IsEmpty(result);
        }     
        
        
        [Test]
        public void DoesNotReturnTransactionsWithALineOfZeroValue()
        {
            var transactionForZero = ForAmount(InPeriod, 0);
            var result = Searching.ExecuteSearch((new SearchWindow<EndingParameters>(new EndingParameters(1),FinancialPeriod )), transactionForZero);
            CollectionAssert.IsEmpty(result);
        }  
        
        
        [Test]
        public void ReturnsTransactionForRoundAmount()
        {
            var transactionForRoundAmount = ForAmount(InPeriod, 1000);
            var result = Searching.ExecuteSearch(new SearchWindow<EndingParameters>(new EndingParameters(1),FinancialPeriod ), transactionForRoundAmount);
            CollectionAssert.AreEquivalent(new[]{transactionForRoundAmount}, result);
        }        
        
        [Test]
        public void ReturnsTransactionWithExactlyTheRightAmountOfZeroes()
        {
            var transactionForRoundAmount = ForAmount(InPeriod, 1000);
            var result = Searching.ExecuteSearch(new SearchWindow<EndingParameters>(new EndingParameters(3),FinancialPeriod ), transactionForRoundAmount);
            CollectionAssert.AreEquivalent(new[]{transactionForRoundAmount}, result);
        }    
        
        
        [Test]
        public void DoesNotReturnTransactionWithOneTooFewZeroes()
        {
            var transactionForRoundAmount = ForAmount(InPeriod, 10000);
            var result = Searching.ExecuteSearch(new SearchWindow<EndingParameters>(new EndingParameters(5),FinancialPeriod ), transactionForRoundAmount);
            CollectionAssert.IsEmpty(result);
        }


        private static Transaction ForAmount(DateTime transactionDate, int amountOfPence)
        {
            var amountOfPounds = ((decimal) amountOfPence)/100;
            return new Transaction(Guid.NewGuid().ToString(), transactionDate, String.Empty, String.Empty, String.Empty, String.Empty,
                new LedgerEntry("a", "a", LedgerEntryType.Cr, amountOfPounds),
                new LedgerEntry("b", "b", LedgerEntryType.Dr, amountOfPounds)
            );
        }
    }
}
