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
    public class UnusualAccountsSearcherTests
    {
        [Test]
        public void ReturnsOnlyJournalsPostedToLessUsedAccounts()
        {
            //given one journal which includes a line to a rare account (one posting)
            var journalPostedToUncommonAccount = PostedTo("b", "e");

            //and a search window for the period "all time", for journals to accounts with <2 postings
            var searchWindow = new SearchWindow<UnusualAccountsParameters>(new UnusualAccountsParameters(2),
                new DateRange(new DateTime(1, 1, 1), new DateTime(3000, 12, 31)));

            //when we do the journal search
            var journals = Searching.ExecuteSearch(searchWindow, PostedTo("a", "b"), PostedTo("b", "a"), journalPostedToUncommonAccount);

            //we end up with only the journal to the rare account
            CollectionAssert.AreEquivalent(new []{journalPostedToUncommonAccount}, journals);
        }        

        [Test]
        public void DoesNotReturnDuplicatesWhenJournalsPostedToTwoUnusualAccounts()
        {
            //given one journal which includes a line to two rare accounts (just one posting each)
            var journalPostedToUncommonAccount = PostedTo("d", "e");

            //and a search window for the period "all time", for journals to accounts with <2 postings
            var searchWindow = new SearchWindow<UnusualAccountsParameters>(new UnusualAccountsParameters(2),
                new DateRange(new DateTime(1, 1, 1), new DateTime(3000, 12, 31)));

            //when we do the journal search
            var journals = Searching.ExecuteSearch(searchWindow, PostedTo("a", "b"), PostedTo("b", "a"), journalPostedToUncommonAccount).ToList();

            //we end up with only one copy of the expected journal - it's not repeated for each rare account it's been posted to.
            CollectionAssert.AreEquivalent(new []{journalPostedToUncommonAccount}, journals);
        }
        
        [Test]
        public void DoesNotReturnJournalsOutsideThePeriod()
        {
            //given one journal which includes a line to two rare accounts, but does not apply to the period
            var journal = PostedTo("d", "e", new DateTime(2000, 4, 5));                

            //and a search window for journals to accounts with <2 postings
            var searchWindow = new SearchWindow<UnusualAccountsParameters>(new UnusualAccountsParameters(2),
                new DateRange(new DateTime(1999, 1, 1), new DateTime(1999, 12, 31)));

            //when we do the journal search
            var journals = Searching.ExecuteSearch(searchWindow, journal).ToList();

            //we don't end up with any journals returned
            CollectionAssert.IsEmpty(journals);
        }   
        
        
        [Test]
        public void JournalsOutsideThePeriodNotUsedToDetermineWhetherAccountCodeIsUnusual()
        {
            //given one journal inside the period to an account
            var journal = PostedTo("a", "b", new DateTime(1999, 1, 1));

            //and a search window for journals to accounts with <2 postings
            var searchWindow = new SearchWindow<UnusualAccountsParameters>(new UnusualAccountsParameters(2),
                new DateRange(new DateTime(1999, 1, 1), new DateTime(1999, 12, 31)));

            //when we do the journal search            
            var journals = Searching.ExecuteSearch(searchWindow, journal,
                PostedTo("a", "b", new DateTime(2000, 1, 1)),
                PostedTo("a", "b", new DateTime(1998, 12, 31)),
                PostedTo("a", "b", new DateTime(2000, 1, 1))).ToList();

            //those journals posted to the account in other periods don't make the account any less unusual.
            CollectionAssert.AreEquivalent(new[]{journal}, journals);
        }

        public static Journal PostedTo(string accountCode1, string accountCode2, DateTime journalDate)
        {
            return new Journal(Guid.NewGuid(), new DateTime(1999, 12, 1), journalDate,
                new[]
                {
                    new JournalLine(accountCode1, accountCode1, JournalType.Cr, 2.2m),
                    new JournalLine(accountCode2, accountCode2, JournalType.Dr, 2.2m)
                });
        }

        private static Journal PostedTo(string accountCode1, string accountCode2)
        {
            return PostedTo(accountCode1, accountCode2, new DateTime(1999, 12, 1));
        }
    }
}
