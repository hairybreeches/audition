using System;
using System.Collections.Generic;
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
        public void DoesNotReturnTransactionsOutsideDaysAllowed()
        {
            var daysApart = 13;
            var transactionsInRepository = new[]
            {
                CreateTransaction(InPeriod, 125, "code1"),
                CreateTransaction(InPeriod.AddDays(daysApart), 125, "code1")
            };
            var results = ExecuteSearch(daysApart - 1, transactionsInRepository);

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
            return new Transaction(Guid.NewGuid(), transactionDate, ledgerEntries);
        }

        private static LedgerEntry CreateLedgerEntry(int amountOfPence, string nominalCode)
        {
            var amountOfPounds = ((decimal) amountOfPence)/100;
            var ledgerEntry = new LedgerEntry(nominalCode, nominalCode, LedgerEntryType.Cr, amountOfPounds);
            return ledgerEntry;
        }
    }
}
