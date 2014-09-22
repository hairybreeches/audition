using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Accounting;
using Model.SearchWindows;
using Model.Time;
using NUnit.Framework;
using Tests.Mocks;

namespace Tests
{
    [TestFixture]
    public class XeroAccountsSearchingTests
    {
        [Test]
        public void ReturnsOnlyJournalsPostedToLessUsedAccounts()
        {
            //given one journal which includes a line to a rare account (one posting)
            var journalPostedToUncommonAccount = JournalPostedTo("b", "e");
            //and several to accounts with >= 2 postings
            var searcher = Create.JournalSearcher(JournalPostedTo("a", "b"), JournalPostedTo("b", "a"), journalPostedToUncommonAccount);

            //and a search window for the period "all time", for journals to accounts with <2 postings
            var searchWindow = new AccountsSearchWindow(2,
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
            var journalPostedToUncommonAccount = JournalPostedTo("d", "e");

            //and several to accounts with >= 2 postings
            var searcher = Create.JournalSearcher(JournalPostedTo("a", "b"), JournalPostedTo("b", "a"), journalPostedToUncommonAccount);

            //and a search window for the period "all time", for journals to accounts with <2 postings
            var searchWindow = new AccountsSearchWindow(2,
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
            var journal = JournalPostedTo("d", "e", new DateTime(2000, 4, 5));                

            //and a search window for journals to accounts with <2 postings
            var searchWindow = new AccountsSearchWindow(2,
                new DateRange(new DateTime(1999, 1, 1), new DateTime(1999, 12, 31)));

            //when we do the journal search
            var searcher = Create.JournalSearcher(journal);
            var journals = searcher.FindJournalsWithin(searchWindow).ToList();

            //we don't end up with any journals returned
            CollectionAssert.IsEmpty(journals);
        }   
        
        
        [Test]
        public void JournalsOutsideThePeriodNotUsedToDetermineWhetherAccountCodeIsUnusual()
        {
            //given one journal inside the period to an account
            var journal = JournalPostedTo("a", "b", new DateTime(1999, 1, 1));
            //and a load posted to the same account, but outside the period
            var searcher = Create.JournalSearcher(journal, 
                JournalPostedTo("a", "b", new DateTime(2000,1,1)),
                JournalPostedTo("a", "b", new DateTime(1998,12,31)),
                JournalPostedTo("a", "b", new DateTime(2000,1,1)));


            //and a search window for journals to accounts with <2 postings
            var searchWindow = new AccountsSearchWindow(2,
                new DateRange(new DateTime(1999, 1, 1), new DateTime(1999, 12, 31)));

            //when we do the journal search            
            var journals = searcher.FindJournalsWithin(searchWindow).ToList();

            //those journals posted to the account in other periods don't make the account any less unusual.
            CollectionAssert.AreEquivalent(new[]{journal}, journals);
        }

        private static Journal JournalPostedTo(string accountCode1, string accountCode2)
        {
            return JournalPostedTo(accountCode1, accountCode2, new DateTime(1999, 12, 1));
        }

        private static Journal JournalPostedTo(string accountCode1, string accountCode2, DateTime journalDate)
        {
            return new Journal(Guid.NewGuid(), new DateTime(1999, 12, 1), journalDate,
                new[]
                {
                    new JournalLine(accountCode1, accountCode1, JournalType.Cr, 2.2m),
                    new JournalLine(accountCode2, accountCode2, JournalType.Dr, 2.2m)
                });
        }
    }
}
