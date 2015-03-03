using System;
using System.Linq;
using Model.Accounting;
using Model.Time;
using NUnit.Framework;
using Searching;
using Searching.SearchWindows;
using Tests.Mocks;

namespace Tests.SearcherTests
{
    [TestFixture]
    public class UnusualNominalCodesSearcherTests
    {
        [Test]
        public void ReturnsOnlyTransactionsPostedToLessUsedAccounts()
        {
            //given one transaction which includes a line to a rare account (one posting)
            var transactionPostedToUncommonAccount = PostedTo("b", "e");

            //and a search window for the period "all time", for transactions to accounts with <2 postings
            var searchWindow = new SearchWindow<UnusualNominalCodesParameters>(new UnusualNominalCodesParameters(2),
                new DateRange(new DateTime(1, 1, 1), new DateTime(3000, 12, 31)));

            //when we do the transaction search
            var transactions = Searching.ExecuteSearch(searchWindow, PostedTo("a", "b"), PostedTo("b", "a"), transactionPostedToUncommonAccount);

            //we end up with only the transaction to the rare account
            CollectionAssert.AreEquivalent(new []{transactionPostedToUncommonAccount}, transactions);
        }        

        [Test]
        public void DoesNotReturnDuplicatesWhenTransactionsPostedToTwoUnusualNominalCodes()
        {
            //given one transaction which includes a line to two rare accounts (just one posting each)
            var transactionPostedToUncommonAccount = PostedTo("d", "e");

            //and a search window for the period "all time", for transactions to accounts with <2 postings
            var searchWindow = new SearchWindow<UnusualNominalCodesParameters>(new UnusualNominalCodesParameters(2),
                new DateRange(new DateTime(1, 1, 1), new DateTime(3000, 12, 31)));

            //when we do the transaction search
            var transactions = Searching.ExecuteSearch(searchWindow, PostedTo("a", "b"), PostedTo("b", "a"), transactionPostedToUncommonAccount).ToList();

            //we end up with only one copy of the expected transaction - it's not repeated for each rare account it's been posted to.
            CollectionAssert.AreEquivalent(new []{transactionPostedToUncommonAccount}, transactions);
        }
        
        [Test]
        public void DoesNotReturnTransactionsOutsideThePeriod()
        {
            //given one transaction which includes a line to two rare accounts, but does not apply to the period
            var transaction = PostedTo("d", "e", new DateTime(2000, 4, 5));                

            //and a search window for transactions to accounts with <2 postings
            var searchWindow = new SearchWindow<UnusualNominalCodesParameters>(new UnusualNominalCodesParameters(2),
                new DateRange(new DateTime(1999, 1, 1), new DateTime(1999, 12, 31)));

            //when we do the transaction search
            var transactions = Searching.ExecuteSearch(searchWindow, transaction).ToList();

            //we don't end up with any transactions returned
            CollectionAssert.IsEmpty(transactions);
        }   
        
        
        [Test]
        public void TransactionsOutsideThePeriodNotUsedToDetermineWhetherNominalCodeIsUnusual()
        {
            //given one transaction inside the period to an account
            var transaction = PostedTo("a", "b", new DateTime(1999, 1, 1));

            //and a search window for transactions to accounts with <2 postings
            var searchWindow = new SearchWindow<UnusualNominalCodesParameters>(new UnusualNominalCodesParameters(2),
                new DateRange(new DateTime(1999, 1, 1), new DateTime(1999, 12, 31)));

            //when we do the transaction search            
            var transactions = Searching.ExecuteSearch(searchWindow, transaction,
                PostedTo("a", "b", new DateTime(2000, 1, 1)),
                PostedTo("a", "b", new DateTime(1998, 12, 31)),
                PostedTo("a", "b", new DateTime(2000, 1, 1))).ToList();

            //those transactions posted to the account in other periods don't make the account any less unusual.
            CollectionAssert.AreEquivalent(new[]{transaction}, transactions);
        }

        public static Transaction PostedTo(string nominalCode1, string nominalCode2, DateTime transactionDate)
        {
            return new Transaction(Guid.NewGuid(), transactionDate,
                new[]
                {
                    new LedgerEntry(nominalCode1, nominalCode1, LedgerEntryType.Cr, 2.2m),
                    new LedgerEntry(nominalCode2, nominalCode2, LedgerEntryType.Dr, 2.2m)
                });
        }

        private static Transaction PostedTo(string nominalCode1, string nominalCode2)
        {
            return PostedTo(nominalCode1, nominalCode2, new DateTime(1999, 12, 1));
        }
    }
}
