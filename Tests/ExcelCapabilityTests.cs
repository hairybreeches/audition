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
        [TestCaseSource("SingleFieldMapped")]
        [TestCaseSource("SingleFieldUnmapped")]
        public SearchCapability GetSearchCapability(FieldLookups lookups)
        {
            var factoryFactory = new ExcelSearcherFactoryFactory(new ExcelDataMapper(new ExcelColumnNamer()));
            var searcherFactory = factoryFactory.CreateSearcherFactory(lookups);
            return searcherFactory.GetSearchCapability();
        }

        public IEnumerable<TestCaseData> SingleFieldMapped
        {
            get
            {
                yield return CreateTestCase(new FieldLookups(id: 1, accountCode: -1, accountName: -1, amount: -1, created: -1, description: -1, journalDate: 18, username: -1), new[] { DisplayField.JournalDate }, new Dictionary<string, string>().WithAllErrorMessages(), "ID mapped");

                yield return CreateTestCase(new FieldLookups(id: -1, accountCode: 12, accountName: -1, amount: -1, created: -1, description: -1, journalDate: 18, username: -1),
                    new[] { DisplayField.JournalDate, DisplayField.AccountCode }, 
                    new Dictionary<string, string>().WithAllErrorMessages().Without(SearchAction.Accounts),
                    "Account Code mapped");

                yield return CreateTestCase(new FieldLookups(id: -1, accountCode: -1, accountName: 3, amount: -1, created: -1, description: -1, journalDate: 18, username: -1),
                        new[] { DisplayField.JournalDate, DisplayField.AccountName },
                        new Dictionary<string, string>().WithAllErrorMessages(),
                        "Account name mapped");

                yield return CreateTestCase(new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: 5, created: -1, description: -1, journalDate: 18, username: -1),
                        new[] { DisplayField.JournalDate, DisplayField.JournalType, DisplayField.Amount }, 
                        new Dictionary<string, string>().WithAllErrorMessages().Without(SearchAction.Ending),
                        "Amount mapped");

                yield return CreateTestCase(new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1, created: 7, description: -1, journalDate: 18, username: -1),
                        new[] { DisplayField.Created, DisplayField.JournalDate }, 
                        new Dictionary<string, string>().WithAllErrorMessages().Without(SearchAction.Date).Without(SearchAction.Hours),
                        "Creation date mapped");

                yield return CreateTestCase(new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1, created: -1, description: 4, journalDate: 18, username: -1),
                        new[] { DisplayField.JournalDate, DisplayField.Description }, 
                        new Dictionary<string, string>().WithAllErrorMessages(),
                        "Description mapped");

                yield return CreateTestCase(new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1, created: -1, description: -1, journalDate: 18, username: 319),
                        new[] { DisplayField.JournalDate, DisplayField.Username }, 
                        new Dictionary<string, string>().WithAllErrorMessages().Without(SearchAction.Users),
                        "Username mapped");
            }
        }

        public IEnumerable<TestCaseData> SingleFieldUnmapped
        {
            get
            {
                yield return CreateTestCase(new FieldLookups(id: -1, accountCode: 18, accountName: 18, amount: 18, created: 18, description: 18, journalDate: 18, username: 18),
                    Enums.GetAllValues<DisplayField>(), new Dictionary<string, string>(),
                    "ID unmapped");

                yield return CreateTestCase(new FieldLookups(id: 18, accountCode: -1, accountName: 18, amount: 18, created: 18, description: 18, journalDate: 18, username: 18),
                    Enums.GetAllValues<DisplayField>().Without(DisplayField.AccountCode), new Dictionary<string, string>().WithAccountsErrorMessage(),
                    "Account Code unmapped");

                yield return CreateTestCase(new FieldLookups(id: 18, accountCode: 18, accountName: -1, amount: 18, created: 18, description: 18, journalDate: 18, username: 18),
                        Enums.GetAllValues<DisplayField>().Without(DisplayField.AccountName), new Dictionary<string, string>(),
                        "Account name unmapped");

                yield return CreateTestCase(new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: -1, created: 18, description: 18, journalDate: 18, username: 18),
                        Enums.GetAllValues<DisplayField>().Without(DisplayField.Amount).Without(DisplayField.JournalType), new Dictionary<string, string>().WithEndingErrorMessage(),
                        "Amount unmapped");

                yield return CreateTestCase(new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: 18, created: -1, description: 18, journalDate: 18, username: 18),
                        Enums.GetAllValues<DisplayField>().Without(DisplayField.Created), new Dictionary<string, string>()
                        .WithHoursErrorMessage()
                        .WithYearEndErrorMessage(),
                        "Journal creation time unmapped");

                yield return CreateTestCase(new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: 18, created: 18, description: -1, journalDate: 18, username: 18),
                        Enums.GetAllValues<DisplayField>().Without(DisplayField.Description), new Dictionary<string, string>(),
                        "Description unmapped");

                yield return CreateTestCase(new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: 18, created: 18, description: 18, journalDate: 18, username: -1),
                        Enums.GetAllValues<DisplayField>().Without(DisplayField.Username), new Dictionary<string, string>().WithUsersErrorMessage(),
                        "Username unmapped");
            }
        }


        private static TestCaseData CreateTestCase(FieldLookups fieldLookups, IList<DisplayField> availableFields, IDictionary<string, string> errorMessages, string name)
        {
            return new TestCaseData(fieldLookups)
                .Returns(new SearchCapability(availableFields, errorMessages))
                .SetName(name);
        }
    }
}
