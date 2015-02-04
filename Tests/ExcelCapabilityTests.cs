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
        [TestCaseSource("SingleFieldMapped")]
        public IEnumerable<DisplayField> WhenSingleFieldMappedDisplayFieldsAvailable(FieldLookups lookups)
        {
            return GetSearchCapability(lookups).AvailableFields;            
        }

        public IEnumerable<TestCaseData> SingleFieldMapped
        {
            get
            {
                yield return new TestCaseData(new FieldLookups(id: 1, accountCode: -1, accountName: -1, amount: -1, created: -1, description: -1, journalDate: -1, username: -1))
                    .Returns(Enumerable.Empty<DisplayField>());
                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: 12, accountName: -1, amount: -1, created: -1, description: -1, journalDate: -1, username: -1))
                    .Returns(new[] { DisplayField.AccountCode });
                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: -1, accountName: 3, amount: -1, created: -1, description: -1, journalDate: -1, username: -1))
                        .Returns(new[] { DisplayField.AccountName });
                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: 5, created: -1, description: -1, journalDate: -1, username: -1))
                        .Returns(new[] { DisplayField.Amount, DisplayField.JournalType, });
                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1, created: 7, description: -1, journalDate: -1, username: -1))
                        .Returns(new[] { DisplayField.Created });
                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1, created: -1, description: 4, journalDate: -1, username: -1))
                        .Returns(new[] { DisplayField.Description });
                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1, created: -1, description: -1, journalDate: 9, username: -1))
                        .Returns(new[] { DisplayField.JournalDate });
                yield return new TestCaseData(new FieldLookups(id: -1, accountCode: -1, accountName: -1, amount: -1, created: -1, description: -1, journalDate: -1, username: 319))
                        .Returns(new[] { DisplayField.Username });
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
