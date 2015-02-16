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
    public class UserSearcherTests
    {
        private static readonly DateTime YearEnd = new DateTime(2012, 3, 31);
        private static readonly DateTime YearStart = YearEnd.Subtract(TimeSpan.FromDays(365));
        private static readonly DateTime InPeriod = YearEnd.Subtract(TimeSpan.FromDays(30));
        private static readonly DateRange FinancialPeriod = new DateRange(YearStart, YearEnd);

        [Test]
        public void DoesNotReturnTransactionsWhichDoNotApplyToTheFinancialPeriod()
        {
            var transactionApplyingToPostYearEnd = CreateTransactionByUser("steve", YearEnd.AddDays(1));
            var transactionApplyingToPreYearstart = CreateTransactionByUser("steve", YearStart.Subtract(TimeSpan.FromDays(1)));

            var result = Searching.ExecuteSearch(new SearchWindow<UserParameters>(new UserParameters("nonexistent"), FinancialPeriod), transactionApplyingToPostYearEnd, transactionApplyingToPreYearstart);
            CollectionAssert.IsEmpty(result, "Neither of the transactions should be returned since they're outside the period");
        }

        [Test]
        public void ReturnsTransactionPostedByOtherUserWhenSingleUserSpecified()
        {
            var transaction = CreateTransactionInPeriodByUser("steve");
            var result = Searching.ExecuteSearch(new SearchWindow<UserParameters>(new UserParameters("alf"),FinancialPeriod), transaction);
            CollectionAssert.AreEqual(new[]{transaction}, result, "The transaction should be returned since it's by an unexpected user");
        }

        [Test]
        public void ReturnsCorrectUsersWhenUserInputComplex()
        {
            var transactionsToFind = new[]
            {
                CreateTransactionInPeriodByUser("francois"),
                CreateTransactionInPeriodByUser("polly"),
                CreateTransactionInPeriodByUser("betty"),
                CreateTransactionInPeriodByUser("francois"),
            };

            var transactions = transactionsToFind.Concat(new[]
            {
                CreateTransactionInPeriodByUser("Elizabeth"), 
                CreateTransactionInPeriodByUser("Elizabeth"), 
                CreateTransactionInPeriodByUser("elizabetH"), 
                CreateTransactionInPeriodByUser("Suzy"), 
                CreateTransactionInPeriodByUser("steVE"), 
                CreateTransactionInPeriodByUser("suzY"), 
            })
                //give them a roughly random ordering
                .OrderBy(x=>x.Id);
            var result = Searching.ExecuteSearch(new SearchWindow<UserParameters>(new UserParameters("\telizabeth \n\tsTeve  \r\n\tsuzy\n"), FinancialPeriod), transactions.ToArray());
            CollectionAssert.AreEquivalent(transactionsToFind, result, "The searcher should return only the transactions by unexpected users"); 
        }
        
        [TestCase("steve", "steve", TestName = "Basic no match case")]
        [TestCase("stevE", "Steve", TestName = "Match is case insensitive")]
        [TestCase("steve", "\r \tsteve", TestName = "Match ignores leading whitespace")]
        [TestCase("steve", "steve \t\r", TestName = "Match ignores trailing whitespace")]
        [TestCase("alf", "steve\nalf", TestName = "Matches when user second in list")]
        public void DoesNotReturnTransactionPostedByUserWhenSingleUserSpecified(string username, string userInput)
        {
            var transaction = CreateTransactionInPeriodByUser(username);
            var result = Searching.ExecuteSearch(new SearchWindow<UserParameters>(new UserParameters(userInput),FinancialPeriod), transaction);
            CollectionAssert.IsEmpty(result, "The transaction should not be returned since it's by an expected user");
        }

        private static Transaction CreateTransactionInPeriodByUser(string user)
        {
            return CreateTransactionByUser(user, InPeriod);
        }

        private static Transaction CreateTransactionByUser(string user, DateTime transactionDate)
        {
            return new Transaction(Guid.NewGuid().ToString(), new DateTimeOffset(), transactionDate, user, String.Empty, Enumerable.Empty<LedgerEntry>());
        }
    }
}
