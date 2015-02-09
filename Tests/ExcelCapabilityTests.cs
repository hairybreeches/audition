﻿using System.Collections.Generic;
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
            var factoryFactory = new FieldLookupInterpreter(new ExcelColumnNamer());
            var searcherFactory = factoryFactory.CreateSearcherFactory(lookups);
            return searcherFactory.GetSearchCapability();
        }

        public IEnumerable<TestCaseData> SingleFieldMapped
        {
            get
            {
                yield return CreateTestCase("ID mapped", 
                    new FieldLookups(id: 1, accountCode: -1, accountName: -1, amount: -1, created: -1, description: -1, journalDate: 18, username: -1), 
                    new Dictionary<string, string>().WithAllErrorMessages(), 
                    DisplayField.JournalDate);

                yield return CreateTestCase("Account Code mapped", 
                    new FieldLookups(id: -1, accountCode: 12, accountName: -1, amount: -1, created: -1, description: -1, journalDate: 18, username: -1), 
                    new Dictionary<string, string>().WithAllErrorMessages().Without(SearchAction.Accounts), 
                    DisplayField.JournalDate, DisplayField.AccountCode);

                yield return CreateTestCase("Account name mapped", 
                    new FieldLookups(id: -1, accountCode: -1, accountName: 3, amount: -1, created: -1, description: -1, journalDate: 18, username: -1), 
                    new Dictionary<string, string>().WithAllErrorMessages(), 
                    DisplayField.JournalDate, DisplayField.AccountName);

                yield return CreateTestCase("Amount mapped", 
                    new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: 5, created: -1, description: -1, journalDate: 18, username: -1), 
                    new Dictionary<string, string>().WithAllErrorMessages().Without(SearchAction.Ending), 
                    DisplayField.JournalDate, DisplayField.JournalType, DisplayField.Amount);

                yield return CreateTestCase("Creation date mapped", 
                    new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1, created: 7, description: -1, journalDate: 18, username: -1), 
                    new Dictionary<string, string>().WithAllErrorMessages().Without(SearchAction.Date).Without(SearchAction.Hours), 
                    DisplayField.Created, DisplayField.JournalDate);

                yield return CreateTestCase("Description mapped", 
                    new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1, created: -1, description: 4, journalDate: 18, username: -1), 
                    new Dictionary<string, string>().WithAllErrorMessages(), 
                    DisplayField.JournalDate, DisplayField.Description);

                yield return CreateTestCase("Username mapped", 
                    new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1, created: -1, description: -1, journalDate: 18, username: 319), 
                    new Dictionary<string, string>().WithAllErrorMessages().Without(SearchAction.Users), 
                    DisplayField.JournalDate, DisplayField.Username);
            }
        }

        public IEnumerable<TestCaseData> AllOrNothing
        {
            get
            {
                yield return CreateTestCase("All fields mapped",
                    new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: 18, created: 18, description: 18,
                        journalDate: 18, username: 18),
                    new Dictionary<string, string>(),
                    Enums.GetAllValues<DisplayField>());

                yield return CreateTestCase("All fields unmapped",
                    new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1, created: -1, description: -1, journalDate: 18, username: -1),
                    new Dictionary<string, string>().WithAllErrorMessages(),
                    DisplayField.JournalDate);
            }
        }

        public IEnumerable<TestCaseData> SingleFieldUnmapped
        {
            get
            {
                yield return CreateTestCase("ID unmapped", 
                    new FieldLookups(id: -1, accountCode: 18, accountName: 18, amount: 18, created: 18, description: 18, journalDate: 18, username: 18), 
                    new Dictionary<string, string>(), 
                    Enums.GetAllValues<DisplayField>());

                yield return CreateTestCase("Account Code unmapped", 
                    new FieldLookups(id: 18, accountCode: -1, accountName: 18, amount: 18, created: 18, description: 18, journalDate: 18, username: 18), 
                    new Dictionary<string, string>().WithAccountsErrorMessage(), 
                    Enums.GetAllValues<DisplayField>().Without(DisplayField.AccountCode));

                yield return CreateTestCase("Account name unmapped", 
                    new FieldLookups(id: 18, accountCode: 18, accountName: -1, amount: 18, created: 18, description: 18, journalDate: 18, username: 18), 
                    new Dictionary<string, string>(), 
                    Enums.GetAllValues<DisplayField>().Without(DisplayField.AccountName));

                yield return CreateTestCase("Amount unmapped", 
                    new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: -1, created: 18, description: 18, journalDate: 18, username: 18), 
                    new Dictionary<string, string>().WithEndingErrorMessage(), 
                    Enums.GetAllValues<DisplayField>().Without(DisplayField.Amount).Without(DisplayField.JournalType));

                yield return CreateTestCase("Journal creation time unmapped", 
                    new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: 18, created: -1, description: 18, journalDate: 18, username: 18), 
                    new Dictionary<string, string>()
                        .WithHoursErrorMessage()
                    .   WithYearEndErrorMessage(), 
                    Enums.GetAllValues<DisplayField>().Without(DisplayField.Created));

                yield return CreateTestCase("Description unmapped", 
                    new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: 18, created: 18, description: -1, journalDate: 18, username: 18), 
                    new Dictionary<string, string>(), 
                    Enums.GetAllValues<DisplayField>().Without(DisplayField.Description));

                yield return CreateTestCase("Username unmapped", 
                    new FieldLookups(id: 18, accountCode: 18, accountName: 18, amount: 18, created: 18, description: 18, journalDate: 18, username: -1), 
                    new Dictionary<string, string>().WithUsersErrorMessage(), 
                    Enums.GetAllValues<DisplayField>().Without(DisplayField.Username));
            }
        }

        private static TestCaseData CreateTestCase(string name, FieldLookups fieldLookups, IDictionary<string, string> errorMessages, params DisplayField[] availableFields)
        {
            return new TestCaseData(fieldLookups)
                .Returns(new SearchCapability(availableFields, errorMessages))
                .SetName(name);
        }
    }
}