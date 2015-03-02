using System.Collections.Generic;
using Capabilities;
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
        [TestCaseSource("AllOrNothing")]
        public SearchCapability GetSearchCapability(FieldLookups lookups)
        {
            var factoryFactory = new FieldLookupInterpreter(new ExcelColumnNamer(), new SearchActionProvider(), new DisplayFieldProvider());
            var searcherFactory = factoryFactory.CreateSearcherFactory(lookups);
            return searcherFactory.GetSearchCapability();
        }

        public IEnumerable<TestCaseData> SingleFieldMapped
        {
            get
            {
                yield return CreateTestCase("ID mapped", 
                    new FieldLookups(id: 1, accountCode: -1, accountName: -1, amount: -1, description: -1, transactionDate: 18, username: -1, type: -1), 
                    new Dictionary<string, string>().WithAllErrorMessages(), 
                    DisplayFieldName.TransactionDate, DisplayFieldName.Id);

                yield return CreateTestCase("Account Code mapped",
                    new FieldLookups(id: -1, accountCode: 12, accountName: -1, amount: -1, description: -1, transactionDate: 18, username: -1, type: -1), 
                    new Dictionary<string, string>().WithAllErrorMessages().Without(SearchActionName.Accounts), 
                    DisplayFieldName.TransactionDate, DisplayFieldName.AccountCode);

                yield return CreateTestCase("Account name mapped",
                    new FieldLookups(id: -1, accountCode: -1, accountName: 3, amount: -1, description: -1, transactionDate: 18, username: -1, type: -1), 
                    new Dictionary<string, string>().WithAllErrorMessages(), 
                    DisplayFieldName.TransactionDate, DisplayFieldName.AccountName);

                yield return CreateTestCase("Amount mapped",
                    new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: 5, description: -1, transactionDate: 18, username: -1, type: -1), 
                    new Dictionary<string, string>().WithAllErrorMessages().Without(SearchActionName.Ending), 
                    DisplayFieldName.TransactionDate, DisplayFieldName.LedgerEntryType, DisplayFieldName.Amount);

                yield return CreateTestCase("Description mapped",
                    new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1,  description: 4, transactionDate: 18, username: -1, type: -1), 
                    new Dictionary<string, string>().WithAllErrorMessages(), 
                    DisplayFieldName.TransactionDate, DisplayFieldName.Description);

                yield return CreateTestCase("Username mapped",
                    new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1,  description: -1, transactionDate: 18, username: 319, type: -1), 
                    new Dictionary<string, string>().WithAllErrorMessages().Without(SearchActionName.Users), 
                    DisplayFieldName.TransactionDate, DisplayFieldName.Username);
                
                yield return CreateTestCase("Transaction type mapped",
                    new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1, description: -1, transactionDate: 18, username: -1, type: 23), 
                    new Dictionary<string, string>().WithAllErrorMessages(), 
                    DisplayFieldName.TransactionDate, DisplayFieldName.Type);
            }
        }

        public IEnumerable<TestCaseData> AllOrNothing
        {
            get
            {
                yield return CreateTestCase("All fields mapped",
                    new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: 18, description: 18,
                        transactionDate: 18, username: 18, type: 18),
                    new Dictionary<string, string>(),
                    Enums.GetAllValues<DisplayFieldName>());

                yield return CreateTestCase("All fields unmapped",
                    new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1, description: -1, transactionDate: 18, username: -1, type: -1),
                    new Dictionary<string, string>().WithAllErrorMessages(),
                    DisplayFieldName.TransactionDate);
            }
        }

        public IEnumerable<TestCaseData> SingleFieldUnmapped
        {
            get
            {
                yield return CreateTestCase("ID unmapped", 
                    new FieldLookups(id: -1, accountCode: 18, accountName: 18, amount: 18, description: 18, transactionDate: 18, username: 18, type: 18), 
                    new Dictionary<string, string>(), 
                    Enums.GetAllValues<DisplayFieldName>().Without(DisplayFieldName.Id));

                yield return CreateTestCase("Account Code unmapped",
                    new FieldLookups(id: 18, accountCode: -1, accountName: 18, amount: 18, description: 18, transactionDate: 18, username: 18, type: 18), 
                    new Dictionary<string, string>().WithAccountsErrorMessage().WithDuplicatesErrorMessage(), 
                    Enums.GetAllValues<DisplayFieldName>().Without(DisplayFieldName.AccountCode));

                yield return CreateTestCase("Account name unmapped",
                    new FieldLookups(id: 18, accountCode: 18, accountName: -1, amount: 18, description: 18, transactionDate: 18, username: 18, type: 18), 
                    new Dictionary<string, string>(), 
                    Enums.GetAllValues<DisplayFieldName>().Without(DisplayFieldName.AccountName));

                yield return CreateTestCase("Amount unmapped",
                    new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: -1, description: 18, transactionDate: 18, username: 18, type: 18), 
                    new Dictionary<string, string>().WithEndingErrorMessage().WithDuplicatesErrorMessage(), 
                    Enums.GetAllValues<DisplayFieldName>().Without(DisplayFieldName.Amount).Without(DisplayFieldName.LedgerEntryType));

                yield return CreateTestCase("Description unmapped",
                    new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: 18, description: -1, transactionDate: 18, username: 18, type: 18), 
                    new Dictionary<string, string>(), 
                    Enums.GetAllValues<DisplayFieldName>().Without(DisplayFieldName.Description));

                yield return CreateTestCase("Username unmapped",
                    new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: 18, description: 18, transactionDate: 18, username: -1, type: 18), 
                    new Dictionary<string, string>().WithUsersErrorMessage(), 
                    Enums.GetAllValues<DisplayFieldName>().Without(DisplayFieldName.Username));       
                
                yield return CreateTestCase("Type unmapped",
                    new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: 18, description: 18, transactionDate: 18, username: 18, type: -1), 
                    new Dictionary<string, string>(), 
                    Enums.GetAllValues<DisplayFieldName>().Without(DisplayFieldName.Type));
            }
        }

        private static TestCaseData CreateTestCase(string name, FieldLookups fieldLookups, IDictionary<string, string> errorMessages, params DisplayFieldName[] availableFields)
        {
            return new TestCaseData(fieldLookups)
                .Returns(new SearchCapability(availableFields, errorMessages))
                .SetName(name);
        }
    }
}
