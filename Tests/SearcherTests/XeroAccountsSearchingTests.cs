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
    public class XeroAccountsSearchingTests
    {
        [Test]
        public void ReturnsOnlyJournalsPostedToLessUsedAccounts()
        {
            //given one journal which includes a line to a rare account (one posting)
            var journalPostedToUncommonAccount = Mock.JournalPostedTo("b", "e");
            //and several to accounts with >= 2 postings
            var searcher = Mock.JournalSearcher(Mock.JournalPostedTo("a", "b"), Mock.JournalPostedTo("b", "a"), journalPostedToUncommonAccount);

            //and a search window for the period "all time", for journals to accounts with <2 postings
            var searchWindow = new SearchWindow<UnusualAccountsParameters>(new UnusualAccountsParameters(2),
                new DateRange(new DateTime(1, 1, 1), new DateTime(3000, 12, 31)));

            //when we do the journal search
            var journals = searcher.FindJournalsWithin(searchWindow);

            //we end up with only the journal to the rare account
            CollectionAssert.AreEquivalent(new []{journalPostedToUncommonAccount}, journals);
        }
        
        [Test]
        public void DoesNotReturnDuplicatesWhenJournalsPostedToTwoUnusualAccounts()
        {
            //given one journal which includes a line to two rare accounts (just one posting each)
            var journalPostedToUncommonAccount = Mock.JournalPostedTo("d", "e");

            //and several to accounts with >= 2 postings
            var searcher = Mock.JournalSearcher(Mock.JournalPostedTo("a", "b"), Mock.JournalPostedTo("b", "a"), journalPostedToUncommonAccount);

            //and a search window for the period "all time", for journals to accounts with <2 postings
            var searchWindow = new SearchWindow<UnusualAccountsParameters>(new UnusualAccountsParameters(2),
                new DateRange(new DateTime(1, 1, 1), new DateTime(3000, 12, 31)));

            //when we do the journal search
            var journals = searcher.FindJournalsWithin(searchWindow).ToList();

            //we end up with only one copy of the expected journal - it's not repeated for each rare account it's been posted to.
            CollectionAssert.AreEquivalent(new []{journalPostedToUncommonAccount}, journals);
        }
        
        [Test]
        public void DoesNotReturnJournalsOutsideThePeriod()
        {
            //given one journal which includes a line to two rare accounts, but does not apply to the period
            var journal = Mock.JournalPostedTo("d", "e", new DateTime(2000, 4, 5));                

            //and a search window for journals to accounts with <2 postings
            var searchWindow = new SearchWindow<UnusualAccountsParameters>(new UnusualAccountsParameters(2),
                new DateRange(new DateTime(1999, 1, 1), new DateTime(1999, 12, 31)));

            //when we do the journal search
            var searcher = Mock.JournalSearcher(journal);
            var journals = searcher.FindJournalsWithin(searchWindow).ToList();

            //we don't end up with any journals returned
            CollectionAssert.IsEmpty(journals);
        }   
        
        
        [Test]
        public void JournalsOutsideThePeriodNotUsedToDetermineWhetherAccountCodeIsUnusual()
        {
            //given one journal inside the period to an account
            var journal = Mock.JournalPostedTo("a", "b", new DateTime(1999, 1, 1));
            //and a load posted to the same account, but outside the period
            var searcher = Mock.JournalSearcher(journal, 
                Mock.JournalPostedTo("a", "b", new DateTime(2000,1,1)),
                Mock.JournalPostedTo("a", "b", new DateTime(1998,12,31)),
                Mock.JournalPostedTo("a", "b", new DateTime(2000,1,1)));


            //and a search window for journals to accounts with <2 postings
            var searchWindow = new SearchWindow<UnusualAccountsParameters>(new UnusualAccountsParameters(2),
                new DateRange(new DateTime(1999, 1, 1), new DateTime(1999, 12, 31)));

            //when we do the journal search            
            var journals = searcher.FindJournalsWithin(searchWindow).ToList();

            //those journals posted to the account in other periods don't make the account any less unusual.
            CollectionAssert.AreEquivalent(new[]{journal}, journals);
        }
    }
}
