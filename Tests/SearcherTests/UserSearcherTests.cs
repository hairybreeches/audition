using System;
using System.Linq;
using Model.Accounting;
using Model.Searching;
using Model.SearchWindows;
using Model.Time;
using NUnit.Framework;
using Tests.Mocks;

namespace Tests.SearcherTests
{
    public class UserSearcherTests
    {
        private static readonly DateTime YearEnd = new DateTime(2012, 3, 31);
        private static readonly DateTime YearStart = YearEnd.Subtract(TimeSpan.FromDays(365));
        private static readonly DateTime InPeriod = YearEnd.Subtract(TimeSpan.FromDays(30));
        private static readonly DateRange FinancialPeriod = new DateRange(YearStart, YearEnd);

        [Test]
        public void DoesNotReturnJournalsWhichDoNotApplyToTheFinancialPeriod()
        {
            var journalApplyingToPostYearEnd = CreateJournalByUser("steve", YearEnd.AddDays(1));
            var journalApplyingToPreYearstart = CreateJournalByUser("steve", YearStart.Subtract(TimeSpan.FromDays(1)));

            var searcher = CreateSearcher(journalApplyingToPostYearEnd, journalApplyingToPreYearstart);
            var result = searcher.FindJournalsWithin(new SearchWindow<UserParameters>(new UserParameters("nonexistent"), FinancialPeriod));
            CollectionAssert.IsEmpty(result, "Neither of the journals should be returned since they're outside the period");
        }

        [Test]
        public void ReturnsJournalPostedByOtherUserWhenSingleUserSpecified()
        {
            var journal = CreateJournalInPeriodByUser("steve");
            var searcher = CreateSearcher(journal);
            var result = searcher.FindJournalsWithin(new SearchWindow<UserParameters>(new UserParameters("alf"),FinancialPeriod));
            CollectionAssert.AreEqual(new[]{journal}, result, "The journal should be returned since it's by an unexpected user");
        }

        [Test]
        public void ReturnsCorrectUsersWhenUserInputComplex()
        {
            var journalsToFind = new[]
            {
                CreateJournalInPeriodByUser("francois"),
                CreateJournalInPeriodByUser("polly"),
                CreateJournalInPeriodByUser("betty"),
                CreateJournalInPeriodByUser("francois"),
            };

            var journals = journalsToFind.Concat(new[]
            {
                CreateJournalInPeriodByUser("Elizabeth"), 
                CreateJournalInPeriodByUser("Elizabeth"), 
                CreateJournalInPeriodByUser("elizabetH"), 
                CreateJournalInPeriodByUser("Suzy"), 
                CreateJournalInPeriodByUser("steVE"), 
                CreateJournalInPeriodByUser("suzY"), 
            })
                //give them a roughly random ordering
                .OrderBy(x=>x.Id);
            var searcher = CreateSearcher(journals.ToArray());
            var result = searcher.FindJournalsWithin(new SearchWindow<UserParameters>(new UserParameters("\telizabeth \n\tsTeve  \r\n\tsuzy\n"), FinancialPeriod));
            CollectionAssert.AreEquivalent(journalsToFind, result, "The searcher should return only the journals by unexpected users"); 
        }
        
        [TestCase("steve", "steve", TestName = "Basic no match case")]
        [TestCase("stevE", "Steve", TestName = "Match is case insensitive")]
        [TestCase("steve", "\r \tsteve", TestName = "Match ignores leading whitespace")]
        [TestCase("steve", "steve \t\r", TestName = "Match ignores trailing whitespace")]
        [TestCase("alf", "steve\nalf", TestName = "Matches when user second in list")]
        public void DoesNotReturnJournalPostedByUserWhenSingleUserSpecified(string username, string userInput)
        {
            var journal = CreateJournalInPeriodByUser(username);
            var searcher = CreateSearcher(journal);
            var result = searcher.FindJournalsWithin(new SearchWindow<UserParameters>(new UserParameters(userInput),FinancialPeriod));
            CollectionAssert.IsEmpty(result, "The journal should not be returned since it's by an expected user");
        }

        private static IJournalSearcher<UserParameters> CreateSearcher(params Journal[] journals)
        {
            return new UserSearcher(new JournalRepository(journals));
        }

        private static Journal CreateJournalInPeriodByUser(string user)
        {
            return CreateJournalByUser(user, InPeriod);
        }

        private static Journal CreateJournalByUser(string user, DateTime journalDate)
        {
            return new Journal(Guid.NewGuid().ToString(), new DateTimeOffset(), journalDate, user, String.Empty, Enumerable.Empty<JournalLine>());
        }
    }
}
