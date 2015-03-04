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
        public void ReturnsOnlyTransactionsPostedToLessUsedNominalCodes()
        {
            //given one transaction which includes a line to a rare nominal code (one posting)
            var transactionPostedToUncommonNominalCode = PostedTo("b", "e");

            //and a search window for the period "all time", for transactions to nominal codes with <2 postings
            var searchWindow = new SearchWindow<UnusualNominalCodesParameters>(new UnusualNominalCodesParameters(2),
                new DateRange(new DateTime(1, 1, 1), new DateTime(3000, 12, 31)));

            //when we do the transaction search
            var transactions = Searching.ExecuteSearch(searchWindow, PostedTo("a", "b"), PostedTo("b", "a"), transactionPostedToUncommonNominalCode);

            //we end up with only the transaction to the rare nominal code
            CollectionAssert.AreEquivalent(new []{transactionPostedToUncommonNominalCode}, transactions);
        }        

        [Test]
        public void ReturnsDuplicatesWhenTransactionsPostedToTwoUnusualNominalCodes()
        {
            //given one transaction which includes a line to two rare nominal codes (just one posting each)
            var postedToTwoUnusualNominalCodes = PostedTo("d", "e");

            var postedToUnusualCode1 = PostedTo("a", "d");
            var postedToUnusualCode2 = PostedTo("b", "e");

            //and a search window for the period "all time", for transactions to nominal codes with <3 postings
            var searchWindow = new SearchWindow<UnusualNominalCodesParameters>(new UnusualNominalCodesParameters(3),
                new DateRange(new DateTime(1, 1, 1), new DateTime(3000, 12, 31)));

            //when we do the transaction search
            var transactions = Searching.ExecuteSearch(searchWindow, postedToUnusualCode1, postedToUnusualCode2, PostedTo("a", "b"), PostedTo("b", "a"), postedToTwoUnusualNominalCodes).ToList();

            //the transaction posted to two unusual codes appears twice, once in the list for nominal code 1 and once in the list for nominal code 2
            CollectionAssert.AreEqual(new []{postedToUnusualCode1, postedToTwoUnusualNominalCodes, postedToUnusualCode2, postedToTwoUnusualNominalCodes}, transactions);
        }
        
        [Test]
        public void DoesNotReturnTransactionsOutsideThePeriod()
        {
            //given one transaction which includes a line to two rare nominal codes, but does not apply to the period
            var transaction = PostedTo("d", "e", new DateTime(2000, 4, 5));                

            //and a search window for transactions to nominal codes with <2 postings
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
            //given one transaction inside the period to a nominal code
            var transaction = PostedTo("a", "b", new DateTime(1999, 1, 1));

            //and a search window for transactions to nominal codes with <2 postings
            var searchWindow = new SearchWindow<UnusualNominalCodesParameters>(new UnusualNominalCodesParameters(2),
                new DateRange(new DateTime(1999, 1, 1), new DateTime(1999, 12, 31)));

            //when we do the transaction search            
            var transactions = Searching.ExecuteSearch(searchWindow, transaction,
                PostedTo("a", "b", new DateTime(2000, 1, 1)),
                PostedTo("a", "b", new DateTime(1998, 12, 31)),
                PostedTo("a", "c", new DateTime(1999, 12, 31)),
                PostedTo("a", "c", new DateTime(1999, 12, 31)),
                PostedTo("a", "b", new DateTime(2000, 1, 1))).ToList();

            //those transactions posted to the nominal code in other periods don't make the code any less unusual.
            CollectionAssert.AreEquivalent(new[]{transaction}, transactions);
        }

        public static Transaction PostedTo(string nominalCode1, string nominalCode2, DateTime transactionDate)
        {
            return new Transaction(Guid.NewGuid().ToString(), transactionDate, String.Empty, String.Empty, String.Empty, 
                new LedgerEntry(nominalCode1, nominalCode1, LedgerEntryType.Cr, 2.2m),
                new LedgerEntry(nominalCode2, nominalCode2, LedgerEntryType.Dr, 2.2m)
            );
        }

        private static Transaction PostedTo(string nominalCode1, string nominalCode2)
        {
            return PostedTo(nominalCode1, nominalCode2, new DateTime(1999, 12, 1));
        }
    }
}
