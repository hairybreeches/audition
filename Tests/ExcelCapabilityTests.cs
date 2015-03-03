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
                    new FieldLookups(id: 1, nominalCode: -1, nominalName: -1, amount: -1, description: -1, transactionDate: 18, username: -1, type: -1, accountCode: -1), 
                    new Dictionary<string, string>().WithAllErrorMessages(), 
                    DisplayFieldName.TransactionDate, DisplayFieldName.Id);

                yield return CreateTestCase("Nominal Code mapped",
                    new FieldLookups(id: -1, nominalCode: 12, nominalName: -1, amount: -1, description: -1, transactionDate: 18, username: -1, type: -1, accountCode: -1), 
                    new Dictionary<string, string>().WithAllErrorMessages().Without(SearchActionName.NominalCodes), 
                    DisplayFieldName.TransactionDate, DisplayFieldName.NominalCode);

                yield return CreateTestCase("Nominal name mapped",
                    new FieldLookups(id: -1, nominalCode: -1, nominalName: 3, amount: -1, description: -1, transactionDate: 18, username: -1, type: -1, accountCode: -1), 
                    new Dictionary<string, string>().WithAllErrorMessages(), 
                    DisplayFieldName.TransactionDate, DisplayFieldName.NominalName);

                yield return CreateTestCase("Amount mapped",
                    new FieldLookups(id: -1, nominalCode: -1, nominalName: -1, amount: 5, description: -1, transactionDate: 18, username: -1, type: -1, accountCode: -1), 
                    new Dictionary<string, string>().WithAllErrorMessages().Without(SearchActionName.Ending), 
                    DisplayFieldName.TransactionDate, DisplayFieldName.LedgerEntryType, DisplayFieldName.Amount);

                yield return CreateTestCase("Description mapped",
                    new FieldLookups(id: -1, nominalCode: -1, nominalName: -1, amount: -1, description: 4, transactionDate: 18, username: -1, type: -1, accountCode: -1), 
                    new Dictionary<string, string>().WithAllErrorMessages(), 
                    DisplayFieldName.TransactionDate, DisplayFieldName.Description);

                yield return CreateTestCase("Username mapped",
                    new FieldLookups(id: -1, nominalCode: -1, nominalName: -1, amount: -1, description: -1, transactionDate: 18, username: 319, type: -1, accountCode: -1), 
                    new Dictionary<string, string>().WithAllErrorMessages().Without(SearchActionName.Users), 
                    DisplayFieldName.TransactionDate, DisplayFieldName.Username);
                
                yield return CreateTestCase("Transaction type mapped",
                    new FieldLookups(id: -1, nominalCode: -1, nominalName: -1, amount: -1, description: -1, transactionDate: 18, username: -1, type: 23, accountCode: -1), 
                    new Dictionary<string, string>().WithAllErrorMessages(), 
                    DisplayFieldName.TransactionDate, DisplayFieldName.Type);
                
                yield return CreateTestCase("Account code mapped",
                    new FieldLookups(id: -1, nominalCode: -1, nominalName: -1, amount: -1, description: -1, transactionDate: 18, username: -1, type: -1, accountCode: 45), 
                    new Dictionary<string, string>().WithAllErrorMessages(), 
                    DisplayFieldName.TransactionDate, DisplayFieldName.AccountCode);
            }
        }

        public IEnumerable<TestCaseData> AllOrNothing
        {
            get
            {
                yield return CreateTestCase("All fields mapped",
                    new FieldLookups(id: 18, nominalCode: 18, nominalName: 18, amount: 18, description: 18,
                        transactionDate: 18, username: 18, type: 18, accountCode: 18),
                    new Dictionary<string, string>(),
                    Enums.GetAllValues<DisplayFieldName>());

                yield return CreateTestCase("All fields unmapped",
                    new FieldLookups(id: -1, nominalCode: -1, nominalName: -1, amount: -1, description: -1, transactionDate: 18, username: -1, type: -1, accountCode: -1),
                    new Dictionary<string, string>().WithAllErrorMessages(),
                    DisplayFieldName.TransactionDate);
            }
        }

        public IEnumerable<TestCaseData> SingleFieldUnmapped
        {
            get
            {
                yield return CreateTestCase("ID unmapped", 
                    new FieldLookups(id: -1, nominalCode: 18, nominalName: 18, amount: 18, description: 18, transactionDate: 18, username: 18, type: 18, accountCode: 18), 
                    new Dictionary<string, string>(), 
                    Enums.GetAllValues<DisplayFieldName>().Without(DisplayFieldName.Id));

                yield return CreateTestCase("Nominal Code unmapped",
                    new FieldLookups(id: 18, nominalCode: -1, nominalName: 18, amount: 18, description: 18, transactionDate: 18, username: 18, type: 18, accountCode: 18), 
                    new Dictionary<string, string>().WithNominalCodesErrorMessage(), 
                    Enums.GetAllValues<DisplayFieldName>().Without(DisplayFieldName.NominalCode));

                yield return CreateTestCase("Nominal name unmapped",
                    new FieldLookups(id: 18, nominalCode: 18, nominalName: -1, amount: 18, description: 18, transactionDate: 18, username: 18, type: 18, accountCode: 18), 
                    new Dictionary<string, string>(), 
                    Enums.GetAllValues<DisplayFieldName>().Without(DisplayFieldName.NominalName));

                yield return CreateTestCase("Amount unmapped",
                    new FieldLookups(id: 18, nominalCode: 18, nominalName: 18, amount: -1, description: 18, transactionDate: 18, username: 18, type: 18, accountCode: 18), 
                    new Dictionary<string, string>().WithEndingErrorMessage(), 
                    Enums.GetAllValues<DisplayFieldName>().Without(DisplayFieldName.Amount).Without(DisplayFieldName.LedgerEntryType));

                yield return CreateTestCase("Description unmapped",
                    new FieldLookups(id: 18, nominalCode: 18, nominalName: 18, amount: 18, description: -1, transactionDate: 18, username: 18, type: 18, accountCode: 18), 
                    new Dictionary<string, string>(), 
                    Enums.GetAllValues<DisplayFieldName>().Without(DisplayFieldName.Description));

                yield return CreateTestCase("Username unmapped",
                    new FieldLookups(id: 18, nominalCode: 18, nominalName: 18, amount: 18, description: 18, transactionDate: 18, username: -1, type: 18, accountCode: 18), 
                    new Dictionary<string, string>().WithUsersErrorMessage(), 
                    Enums.GetAllValues<DisplayFieldName>().Without(DisplayFieldName.Username));       
                
                yield return CreateTestCase("Type unmapped",
                    new FieldLookups(id: 18, nominalCode: 18, nominalName: 18, amount: 18, description: 18, transactionDate: 18, username: 18, type: -1, accountCode: 18), 
                    new Dictionary<string, string>(), 
                    Enums.GetAllValues<DisplayFieldName>().Without(DisplayFieldName.Type));  
                
                yield return CreateTestCase("Account code unmapped",
                    new FieldLookups(id: 18, nominalCode: 18, nominalName: 18, amount: 18, description: 18, transactionDate: 18, username: 18, type: 18, accountCode: -1), 
                    new Dictionary<string, string>(), 
                    Enums.GetAllValues<DisplayFieldName>().Without(DisplayFieldName.AccountCode));
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
