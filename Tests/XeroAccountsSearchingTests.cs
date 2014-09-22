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

        private static Journal JournalPostedTo(string accountCode1, string accountCode2)
        {
            return new Journal(Guid.NewGuid(), new DateTime(1999,12,1), new DateTime(1999,12,1),
                new[]
                {
                    new JournalLine(accountCode1, accountCode1, JournalType.Cr, 2.2m),
                    new JournalLine(accountCode2, accountCode2, JournalType.Dr, 2.2m)
                });
        }
    }
}
