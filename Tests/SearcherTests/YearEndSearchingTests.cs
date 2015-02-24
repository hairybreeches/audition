using System;
using System.Linq;
using Model.Accounting;
using Model.Time;
using NUnit.Framework;
using Searching;
using Searching.SearchWindows;

namespace Tests.SearcherTests
{
    [TestFixture]
    public class YearEndSearchingTests
    {
        private static readonly DateTime YearEnd = new DateTime(2012, 3, 31);
        private static readonly DateTime YearStart = YearEnd.Subtract(TimeSpan.FromDays(365));
        private static readonly DateRange FinancialPeriod = new DateRange(YearStart, YearEnd);

        private static readonly SearchWindow<YearEndParameters> SearchParameters = new SearchWindow<YearEndParameters>(new YearEndParameters(5, FinancialPeriod.To),
                FinancialPeriod);

        

        [Test]
        public void ReturnsTransactionsPostedAfterYearEnd()
        {            
            var postYearEndTransaction = new Transaction(Guid.NewGuid(), YearEnd.AddDays(1), YearEnd.Subtract(TimeSpan.FromDays(60)),Enumerable.Empty<LedgerEntry>() );
            var result = Searching.ExecuteSearch(SearchParameters, postYearEndTransaction);
            CollectionAssert.AreEquivalent(new []{postYearEndTransaction}, result);
        }
        
        [Test]
        public void ReturnsTransactionsPostedNearYearEnd()
        {            
            var nearYearEndTransaction = new Transaction(Guid.NewGuid(), YearEnd.Subtract(TimeSpan.FromDays(2)), YearEnd.Subtract(TimeSpan.FromDays(60)),Enumerable.Empty<LedgerEntry>() );
            var result = Searching.ExecuteSearch(SearchParameters, nearYearEndTransaction);
            CollectionAssert.AreEquivalent(new []{nearYearEndTransaction}, result);
        }
        
        [Test]
        public void ReturnsTransactionsPostedExactlyNumberOfDaysBeforeYearEnd()
        {            
            var nearYearEndTransaction = new Transaction(Guid.NewGuid(), YearEnd.Subtract(TimeSpan.FromDays(7)), YearEnd.Subtract(TimeSpan.FromDays(60)),Enumerable.Empty<LedgerEntry>() );
            var result = Searching.ExecuteSearch(new SearchWindow<YearEndParameters>(new YearEndParameters(7, YearEnd), FinancialPeriod), nearYearEndTransaction);        
            CollectionAssert.AreEquivalent(new []{nearYearEndTransaction}, result);
        }

        [Test]
        public void DoesNotReturnTransactionsWhichDoNotApplyToTheFinancialPeriod()
        {
            var timeInsideTheFinancialPeriod = YearEnd.Subtract(TimeSpan.FromDays(2));
            var transactionApplyingToPostYearEnd = new Transaction(Guid.NewGuid(), timeInsideTheFinancialPeriod, YearEnd.AddDays(1), Enumerable.Empty<LedgerEntry>());
            var transactionApplyingToPreYearstart = new Transaction(Guid.NewGuid(), timeInsideTheFinancialPeriod, YearStart.Subtract(TimeSpan.FromDays(1)), Enumerable.Empty<LedgerEntry>());

            var result = Searching.ExecuteSearch(SearchParameters, transactionApplyingToPostYearEnd, transactionApplyingToPreYearstart);
            CollectionAssert.IsEmpty(result);
        }
        
        [Test]
        public void DoesNotReturnTransactionsWhichAreInThePeriodAndNotCloseToYearEnd()
        {
            var transactionNotNearEnoughToYearEnd = new Transaction(Guid.NewGuid(), YearEnd.Subtract(TimeSpan.FromDays(6)), YearEnd.Subtract(TimeSpan.FromDays(60)), Enumerable.Empty<LedgerEntry>());

            var result = Searching.ExecuteSearch(SearchParameters, transactionNotNearEnoughToYearEnd);
            CollectionAssert.IsEmpty(result);
        }
    }
}
