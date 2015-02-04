using System;
using System.Collections.Generic;
using System.Linq;
using ExcelImport;
using Model;
using NUnit.Framework;
using Searching;

namespace Tests
{
    [TestFixture]
    public class ExcelCapabilityTests
    {
        private const string EndingSearchUnavailableMessage = "In order to search for journals with round number endings, you must import journals with an amount value";
        private const string UserSearchUnavailableMessage = "In order to search for journals posted by unexpected users, you must import journals with a username value";
        private const string DateUnavailableMessage = "In order to search for journals created near or after the year end, you must import journals with a creation time value";
        private const string HoursUnavailableMessage = "In order to search for journals posted outside of working hours, you must import journals with a creation time value";
        private const string AccountSearchUnavailableMessage = "In order to search for journals posted to unusual nomincal codes, you must import journals with a nominal code value";

        [TestCaseSource("SingleFieldMapped")]
        public IEnumerable<DisplayField> WhenSingleFieldMappedDisplayFieldsAvailable(FieldLookups lookups)
        {
            return GetSearchCapability(lookups).AvailableFields;
        }

        public IEnumerable<TestCaseData> SingleFieldMapped
        {
            get
            {
                yield return new TestCaseData(new FieldLookups(id: 1, accountCode: -1, accountName: -1, amount: -1, created: -1, description: -1, journalDate: 18, username: -1))
                    .Returns(new[] { DisplayField.JournalDate })
                    .SetName("Display fields: ID mapped");

                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: 12, accountName: -1, amount: -1, created: -1, description: -1, journalDate: 18, username: -1))
                    .Returns(new[] { DisplayField.JournalDate, DisplayField.AccountCode })
                    .SetName("Display fields: Account Code mapped");

                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: -1, accountName: 3, amount: -1, created: -1, description: -1, journalDate: 18, username: -1))
                        .Returns(new[] { DisplayField.JournalDate, DisplayField.AccountName })
                        .SetName("Display fields: Account name mapped");

                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: 5, created: -1, description: -1, journalDate: 18, username: -1))
                        .Returns(new[] { DisplayField.JournalDate, DisplayField.JournalType,DisplayField.Amount })
                        .SetName("Display fields: Amount mapped");

                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1, created: 7, description: -1, journalDate: 18, username: -1))
                        .Returns(new[] { DisplayField.Created, DisplayField.JournalDate })
                        .SetName("Display fields: Creation date mapped");

                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1, created: -1, description: 4, journalDate: 18, username: -1))
                        .Returns(new[] { DisplayField.JournalDate, DisplayField.Description})
                        .SetName("Display fields: Description mapped");

                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1, created: -1, description: -1, journalDate: 18, username: 319))
                        .Returns(new[] { DisplayField.JournalDate, DisplayField.Username})
                        .SetName("Display fields: Username mapped");
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
                    .Returns(new Dictionary<string, string>
                    {
                        {SearchAction.Accounts.ToString(), AccountSearchUnavailableMessage}
                    })
                    .SetName("Error messages: Account Code unmapped");

                yield return new TestCaseData(new FieldLookups(id: 18, accountCode: 18, accountName: -1, amount: 18, created: 18, description: 18, journalDate: 18, username: 18))
                        .Returns(new Dictionary<string, string>())
                        .SetName("Error messages: Account name unmapped");

                yield return new TestCaseData(new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: -1, created: 18, description: 18, journalDate: 18, username: 18))
                        .Returns(new Dictionary<string, string>
                    {
                        {SearchAction.Ending.ToString(), EndingSearchUnavailableMessage}
                    })
                        .SetName("Error messages: Amount unmapped");

                yield return new TestCaseData(new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: 18, created: -1, description: 18, journalDate: 18, username: 18))
                        .Returns(new Dictionary<string, string>
                    {
                        {SearchAction.Hours.ToString(), HoursUnavailableMessage},
                        {SearchAction.Date.ToString(), DateUnavailableMessage}
                    })
                        .SetName("Error messages: Journal creation time unmapped");

                yield return new TestCaseData(new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: 18, created: 18, description: -1, journalDate: 18, username: 18))
                        .Returns(new Dictionary<string, string>())
                        .SetName("Error messages: Description unmapped");

                yield return new TestCaseData(new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: 18, created: 18, description: 18, journalDate: 18, username: -1))
                        .Returns(new Dictionary<string, string>
                    {
                        {SearchAction.Users.ToString(), UserSearchUnavailableMessage}
                    })
                        .SetName("Error messages: Username unmapped");
            }
        }


        public SearchCapability GetSearchCapability(FieldLookups lookups)
        {
            var factoryFactory = new ExcelSearcherFactoryFactory(new ExcelDataMapper(new ExcelColumnNamer()));
            var searcherFactory = factoryFactory.CreateSearcherFactory(lookups);
            return searcherFactory.GetSearchCapability();
        }
    }


}
