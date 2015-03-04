using System;
using System.Collections.Generic;
using System.Linq;
using Model.Accounting;
using Model.Time;
using NUnit.Framework;
using Searching;
using Searching.SearchWindows;

namespace Tests.SearcherTests
{
    [TestFixture]
    public class DuplicatePaymentsSearcherTests
    {
        private static readonly DateTime YearEnd = new DateTime(2012, 3, 31);
        private static readonly DateTime YearStart = YearEnd.Subtract(TimeSpan.FromDays(365));
        private static readonly DateTime InPeriod = YearEnd.Subtract(TimeSpan.FromDays(30));
        private static readonly DateRange FinancialPeriod = new DateRange(YearStart, YearEnd);

        [Test]
        public void ReturnsTransactionOnSameDay()
        {
            var transactionsInRepository = new[]
            {
                CreateTransaction(InPeriod, 250, "code"),
                CreateTransaction(InPeriod, 250, "code")
            };
            var results = ExecuteSearch(31, transactionsInRepository);

            CollectionAssert.AreEquivalent(transactionsInRepository, results);
        }   
        
        
        [Test]
        public void ReturnsTransactionsWhenAMixtureOfDistancesBetween()
        {
            //note this includes all the possible cases apart from first with following outside and last with previous outside - this is covered by DoesNotReturnTransactionsOutsideDaysAllowed
            var noneBeforeWithinAfter = CreateTransaction(YearStart, 250, "code");
            var withinBeforeWithinAfter = CreateTransaction(YearStart.AddDays(2), 250, "code");
            var withinBeforeOutsideAfter = CreateTransaction(YearStart.AddDays(7), 250, "code");
            var outsideBeforeOutsideAfter = CreateTransaction(YearStart.AddDays(14), 250, "code");
            var outsideBeforeWithinAfter = CreateTransaction(YearStart.AddDays(20), 250, "code");
            var withinBeforeNoneAfter = CreateTransaction(YearStart.AddDays(23), 250, "code");  
          
            var results = new HashSet<Transaction>(ExecuteSearch(5, noneBeforeWithinAfter, withinBeforeWithinAfter, withinBeforeNoneAfter, outsideBeforeWithinAfter, outsideBeforeOutsideAfter, withinBeforeOutsideAfter));

            CollectionAssert.Contains(results, noneBeforeWithinAfter, "Search should return the first value when the second value is within range");
            CollectionAssert.Contains(results, withinBeforeWithinAfter, "Search should return a value when the previous and following values are within range");
            CollectionAssert.Contains(results, withinBeforeOutsideAfter, "Search should return a value when the previous value is within range even if the following value is not");
            CollectionAssert.Contains(results, outsideBeforeWithinAfter, "Search should return a value when the following value is within range even if the previous value is not");
            CollectionAssert.Contains(results, withinBeforeNoneAfter, "Search should return the last value when the previous value is within range");
            CollectionAssert.AreEqual(results, new[]{noneBeforeWithinAfter, withinBeforeWithinAfter, withinBeforeOutsideAfter, outsideBeforeWithinAfter, withinBeforeNoneAfter}, "No duplicates should be returned and the results should be returned in date order");
        }

        [Test]
        public void ReturnsAllLinesOfTransactions()
        {
            var transactionsInRepository = new[]
            {
                CreateTransaction(InPeriod, CreateLedgerEntry(250, "code1"), CreateLedgerEntry(250, "code2")),
                CreateTransaction(InPeriod, CreateLedgerEntry(250, "code3"), CreateLedgerEntry(250, "code1"))

            };

            var results = ExecuteSearch(23, transactionsInRepository);

            CollectionAssert.AreEqual(transactionsInRepository, results);
        }      
        
        [Test]
        public void ReturnsDuplicatesWhenCoincidesWithTwoGroups()
        {
            var transaction1 = CreateTransaction(InPeriod, CreateLedgerEntry(125, "code1"), CreateLedgerEntry(36, "code2"));
            var transaction2 = CreateTransaction(InPeriod, CreateLedgerEntry(74, "code3"), CreateLedgerEntry(125, "code1"));
            var transaction3 = CreateTransaction(InPeriod, CreateLedgerEntry(36, "code2"), CreateLedgerEntry(250, "code4"));

            var results = ExecuteSearch(23, transaction1, transaction2, transaction3).ToList();

            CollectionAssert.AreEqual(new[]
            {
                transaction1,
                transaction2,                
                transaction1,
                transaction3
            }, results);
        }

        [Test]
        public void DoesNotReturnTransactionsOutsideDaysAllowed()
        {
            //note this is the case not covered by ReturnsTransactionsWhenAMixtureOfDistancesBetween
            var daysApart = 13;
            var transactionsInRepository = new[]
            {
                CreateTransaction(InPeriod, 125, "code1"),
                CreateTransaction(InPeriod.AddDays(daysApart), 125, "code1")
            };
            var results = ExecuteSearch(daysApart - 1, transactionsInRepository);

            CollectionAssert.IsEmpty(results);
        }     
        
        [Test]
        public void DoesNotReturnTransactionsWhenSignDifferent()
        {
            var transactionsInRepository = new[]
            {
                CreateTransaction(InPeriod, new LedgerEntry("code1", "descrip", LedgerEntryType.Dr, 125)),
                CreateTransaction(InPeriod, new LedgerEntry("code1", "descrip", LedgerEntryType.Cr, 125))
            };
            var results = ExecuteSearch(25, transactionsInRepository);

            CollectionAssert.IsEmpty(results);
        }  
        
        [Test]
        public void DoesNotReturnTransactionsForDifferentAmounts()
        {
            var transactionsInRepository = new[]
            {
                CreateTransaction(InPeriod, 250, "code"),
                CreateTransaction(InPeriod, 125, "code")
            };
            var results = ExecuteSearch(31, transactionsInRepository);

            CollectionAssert.IsEmpty(results);
        }    
                
        [Test]
        public void DoesNotReturnTransactionsToDifferentCodes()
        {
            var transactionsInRepository = new[]
            {
                CreateTransaction(InPeriod, 125, "code1"),
                CreateTransaction(InPeriod, 125, "code2")
            };
            var results = ExecuteSearch(31, transactionsInRepository);

            CollectionAssert.IsEmpty(results);
        }     
        
        [Test]
        public void ReturnsTransactionsOnEdgeOfDaysAllowed()
        {
            var daysApart = 25;
            var transactionsInRepository = new[]
            {
                CreateTransaction(InPeriod, 125, "code1"),
                CreateTransaction(InPeriod.AddDays(daysApart), 125, "code1")
            };
            var results = ExecuteSearch(daysApart, transactionsInRepository);

            CollectionAssert.AreEquivalent(transactionsInRepository, results);
        }                     
        
        
        [Test]
        public void DoesNotReturnDuplicatesWithZeroValues()
        {
            var transactionsInRepository = new[]
            {
                CreateTransaction(InPeriod, 0, "code1"),
                CreateTransaction(InPeriod, 0, "code1")
            };
            var results = ExecuteSearch(23, transactionsInRepository);

            CollectionAssert.IsEmpty(results);
        }
        
        [Test]
        public void DoesNotReturnDuplicatesWhenBothOutsidePeriod()
        {
            var transactionsInRepository = new[]
            {
                CreateTransaction(YearEnd.AddDays(1), 34, "code1"),
                CreateTransaction(YearEnd.AddDays(1), 34, "code1")
            };
            var results = ExecuteSearch(23, transactionsInRepository);

            CollectionAssert.IsEmpty(results);
        }   
        
        [Test]
        public void DoesNotReturnDuplicatesWhenOneOutsidePeriod()
        {
            var transactionsInRepository = new[]
            {
                CreateTransaction(YearStart.Subtract(TimeSpan.FromDays(1)), 34, "code1"),
                CreateTransaction(YearStart.AddDays(1), 34, "code1")
            };
            var results = ExecuteSearch(23, transactionsInRepository);

            CollectionAssert.IsEmpty(results);
        }

        private static IEnumerable<Transaction> ExecuteSearch(int maximumDaysBetweenTransactions, params Transaction[] transactionsInRepository)
        {
            return Searching.ExecuteSearch(new SearchWindow<DuplicatePaymentsParameters>(new DuplicatePaymentsParameters(maximumDaysBetweenTransactions), FinancialPeriod), transactionsInRepository);
        }

        private static Transaction CreateTransaction(DateTime transactionDate, int amountOfPence, string nominalCode)
        {
            var ledgerEntry = CreateLedgerEntry(amountOfPence, nominalCode);
            return CreateTransaction(transactionDate, ledgerEntry);
        }

        private static Transaction CreateTransaction(DateTime transactionDate, params LedgerEntry[] ledgerEntries)
        {
            return new Transaction(Guid.NewGuid().ToString(), transactionDate, String.Empty, String.Empty, String.Empty, String.Empty, ledgerEntries);
        }

        private static LedgerEntry CreateLedgerEntry(int amountOfPence, string nominalCode)
        {
            var amountOfPounds = ((decimal) amountOfPence)/100;
            var ledgerEntry = new LedgerEntry(nominalCode, nominalCode, LedgerEntryType.Cr, amountOfPounds);
            return ledgerEntry;
        }
    }
}
