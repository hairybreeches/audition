using System.Collections.Generic;
using ExcelImport;
using Model;
using NUnit.Framework;
using Searching;

namespace Tests
{
    [TestFixture]
    public class ExcelCapabilityTests
    {
        public IEnumerable<TestCaseData> SingleFieldMapped
        {
            get
            {
                yield return new TestCaseData(new FieldLookups(id: 1, accountCode: -1, accountName: -1, amount: -1, created: -1, description: -1, journalDate: 18, username: -1))
                    .Returns(new SearchCapability(new[] { DisplayField.JournalDate }, new Dictionary<string, string>().WithAllErrorMessages()))
                    .SetName("ID mapped");

                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: 12, accountName: -1, amount: -1, created: -1, description: -1, journalDate: 18, username: -1))
                    .Returns(new SearchCapability(new[] { DisplayField.JournalDate, DisplayField.AccountCode }, new Dictionary<string, string>().WithAllErrorMessages().Without(SearchAction.Accounts)))
                    .SetName("Account Code mapped");

                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: -1, accountName: 3, amount: -1, created: -1, description: -1, journalDate: 18, username: -1))
                        .Returns(new SearchCapability(new[] { DisplayField.JournalDate, DisplayField.AccountName },new Dictionary<string, string>().WithAllErrorMessages()))
                        .SetName("Account name mapped");

                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: 5, created: -1, description: -1, journalDate: 18, username: -1))
                        .Returns(new SearchCapability(new[] { DisplayField.JournalDate, DisplayField.JournalType, DisplayField.Amount }, new Dictionary<string, string>().WithAllErrorMessages().Without(SearchAction.Ending)))
                        .SetName("Amount mapped");

                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1, created: 7, description: -1, journalDate: 18, username: -1))
                        .Returns(new SearchCapability(new[] { DisplayField.Created, DisplayField.JournalDate }, new Dictionary<string, string>().WithAllErrorMessages().Without(SearchAction.Date).Without(SearchAction.Hours)))
                        .SetName("Creation date mapped");

                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1, created: -1, description: 4, journalDate: 18, username: -1))
                        .Returns(new SearchCapability(new[] { DisplayField.JournalDate, DisplayField.Description }, new Dictionary<string, string>().WithAllErrorMessages()))
                        .SetName("Description mapped");

                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1, created: -1, description: -1, journalDate: 18, username: 319))
                        .Returns(new SearchCapability(new[] { DisplayField.JournalDate, DisplayField.Username }, new Dictionary<string, string>().WithAllErrorMessages().Without(SearchAction.Users)))
                        .SetName("Username mapped");
            }
        }

        [TestCaseSource("SingleFieldUnmapped")]
        public IDictionary<string, string> WhenSingleFieldUnmappedCorrectErrorMessageReceived(FieldLookups lookups)
        {
            return GetSearchCapability(lookups).UnvailableActionMessages;
        }

        public IEnumerable<TestCaseData> SingleFieldUnmapped
        {
            get
            {
                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: 18, accountName: 18, amount: 18, created: 18, description: 18, journalDate: 18, username: 18))
                    .Returns(new Dictionary<string, string>())
                    .SetName("Error messages: ID unmapped");

                yield return new TestCaseData(new FieldLookups(id: 18, accountCode: -1, accountName: 18, amount: 18, created: 18, description: 18, journalDate: 18, username: 18))
                    .Returns(new Dictionary<string, string>().WithAccountsErrorMessage())
                    .SetName("Error messages: Account Code unmapped");

                yield return new TestCaseData(new FieldLookups(id: 18, accountCode: 18, accountName: -1, amount: 18, created: 18, description: 18, journalDate: 18, username: 18))
                        .Returns(new Dictionary<string, string>())
                        .SetName("Error messages: Account name unmapped");

                yield return new TestCaseData(new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: -1, created: 18, description: 18, journalDate: 18, username: 18))
                        .Returns(new Dictionary<string, string>().WithEndingErrorMessage())
                        .SetName("Error messages: Amount unmapped");

                yield return new TestCaseData(new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: 18, created: -1, description: 18, journalDate: 18, username: 18))
                        .Returns(new Dictionary<string, string>()
                        .WithHoursErrorMessage()
                        .WithYearEndErrorMessage())
                        .SetName("Error messages: Journal creation time unmapped");

                yield return new TestCaseData(new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: 18, created: 18, description: -1, journalDate: 18, username: 18))
                        .Returns(new Dictionary<string, string>())
                        .SetName("Error messages: Description unmapped");

                yield return new TestCaseData(new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: 18, created: 18, description: 18, journalDate: 18, username: -1))
                        .Returns(new Dictionary<string, string>().WithUsersErrorMessage())
                        .SetName("Error messages: Username unmapped");
            }


        }
        [TestCaseSource("SingleFieldMapped")]
        public SearchCapability GetSearchCapability(FieldLookups lookups)
        {
            var factoryFactory = new ExcelSearcherFactoryFactory(new ExcelDataMapper(new ExcelColumnNamer()));
            var searcherFactory = factoryFactory.CreateSearcherFactory(lookups);
            return searcherFactory.GetSearchCapability();
        }
    }

    static class ErrorMessageDictionaryExtensions
    {
        private const string EndingSearchUnavailableMessage = "In order to search for journals with round number endings, you must import journals with an amount value";
        private const string UserSearchUnavailableMessage = "In order to search for journals posted by unexpected users, you must import journals with a username value";
        private const string DateUnavailableMessage = "In order to search for journals created near or after the year end, you must import journals with a creation time value";
        private const string HoursUnavailableMessage = "In order to search for journals posted outside of working hours, you must import journals with a creation time value";
        private const string AccountSearchUnavailableMessage = "In order to search for journals posted to unusual nomincal codes, you must import journals with a nominal code value";

        public static IDictionary<string, string> WithAccountsErrorMessage(this IDictionary<string, string> dictionary)
        {
            dictionary.Add(SearchAction.Accounts.ToString(), AccountSearchUnavailableMessage);
            return dictionary;
        }
        public static IDictionary<string, string> WithUsersErrorMessage(this IDictionary<string, string> dictionary)
        {
            dictionary.Add(SearchAction.Users.ToString(), UserSearchUnavailableMessage);
            return dictionary;
        }
        public static IDictionary<string, string> WithEndingErrorMessage(this IDictionary<string, string> dictionary)
        {
            dictionary.Add(SearchAction.Ending.ToString(), EndingSearchUnavailableMessage);
            return dictionary;
        }
        public static IDictionary<string, string> WithYearEndErrorMessage(this IDictionary<string, string> dictionary)
        {
            dictionary.Add(SearchAction.Date.ToString(), DateUnavailableMessage);
            return dictionary;
        }

        public static IDictionary<string, string> WithHoursErrorMessage(this IDictionary<string, string> dictionary)
        {
            dictionary.Add(SearchAction.Hours.ToString(), HoursUnavailableMessage);
            return dictionary;
        }

        public static IDictionary<string, string> WithAllErrorMessages(this IDictionary<string, string> dictionary)
        {
            dictionary.WithAccountsErrorMessage()
                .WithEndingErrorMessage()
                .WithHoursErrorMessage()
                .WithUsersErrorMessage()
                .WithYearEndErrorMessage();

            return dictionary;
        } 

        public static IDictionary<string, string> Without(this IDictionary<string, string> dictionary, SearchAction action)
        {
            dictionary.Remove(action.ToString());
            return dictionary;
        } 
    }


}
