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

        public IEnumerable<TestCaseData> SingleFieldUnmapped
        {
            get
            {
                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: 18, accountName: 18, amount: 18, created: 18, description: 18, journalDate: 18, username: 18))
                    .Returns(new SearchCapability(Enums.GetAllValues<DisplayField>(), new Dictionary<string, string>()))
                    .SetName("ID unmapped");

                yield return new TestCaseData(new FieldLookups(id: 18, accountCode: -1, accountName: 18, amount: 18, created: 18, description: 18, journalDate: 18, username: 18))
                    .Returns(new SearchCapability(Enums.GetAllValues<DisplayField>().Without(DisplayField.AccountCode), new Dictionary<string, string>().WithAccountsErrorMessage()))
                    .SetName("Account Code unmapped");

                yield return new TestCaseData(new FieldLookups(id: 18, accountCode: 18, accountName: -1, amount: 18, created: 18, description: 18, journalDate: 18, username: 18))
                        .Returns(new SearchCapability(Enums.GetAllValues<DisplayField>().Without(DisplayField.AccountName), new Dictionary<string, string>()))
                        .SetName("Account name unmapped");

                yield return new TestCaseData(new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: -1, created: 18, description: 18, journalDate: 18, username: 18))
                        .Returns(new SearchCapability(Enums.GetAllValues<DisplayField>().Without(DisplayField.Amount).Without(DisplayField.JournalType), new Dictionary<string, string>().WithEndingErrorMessage()))
                        .SetName("Amount unmapped");

                yield return new TestCaseData(new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: 18, created: -1, description: 18, journalDate: 18, username: 18))
                        .Returns(new SearchCapability(Enums.GetAllValues<DisplayField>().Without(DisplayField.Created), new Dictionary<string, string>()
                        .WithHoursErrorMessage()
                        .WithYearEndErrorMessage()))
                        .SetName("Journal creation time unmapped");

                yield return new TestCaseData(new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: 18, created: 18, description: -1, journalDate: 18, username: 18))
                        .Returns(new SearchCapability(Enums.GetAllValues<DisplayField>().Without(DisplayField.Description), new Dictionary<string, string>()))
                        .SetName("Description unmapped");

                yield return new TestCaseData(new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: 18, created: 18, description: 18, journalDate: 18, username: -1))
                        .Returns(new SearchCapability(Enums.GetAllValues<DisplayField>().Without(DisplayField.Username), new Dictionary<string, string>().WithUsersErrorMessage()))
                        .SetName("Username unmapped");
            }


        }
        [TestCaseSource("SingleFieldMapped")]
        [TestCaseSource("SingleFieldUnmapped")]
        public SearchCapability GetSearchCapability(FieldLookups lookups)
        {
            var factoryFactory = new ExcelSearcherFactoryFactory(new ExcelDataMapper(new ExcelColumnNamer()));
            var searcherFactory = factoryFactory.CreateSearcherFactory(lookups);
            return searcherFactory.GetSearchCapability();
        }
    }
}
