using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model.Accounting;
using NUnit.Framework;
using Sage50.Parsing;
using SqlImport;

namespace Tests
{
    [TestFixture]
    public class SageTransactionParsingTests
    {
        private static readonly IDictionary<string, string> nominalCodeLookup = new Dictionary<string, string>
        {
            {"1200", "Bank Current Account"},
            {"9998", "Suspense Account"},
            {"2200", "Sales Tax Control Account"}
        };


        [Test]
        public void CanConvertJournals()
        {
            var journals = ParseJournals(
                new object[] { "26", "MANAGER", new DateTime(2013, 12, 31), new DateTime(2010, 4, 27, 17, 16, 0), "1200", "55", "Unpresented Cheque" },
                new object[] { "26", "MANAGER", new DateTime(2013, 12, 31), new DateTime(2010, 4, 27, 17, 16, 0), "9998", "-55", "Unpresented Cheque" },
                new object[] { "26", "MANAGER", new DateTime(2013, 12, 31), new DateTime(2010, 4, 27, 17, 16, 0), "2200", "0", "Unpresented Cheque" },
                new object[] { "12", "Steve", new DateTime(2013, 12, 31), new DateTime(2010, 4, 27, 17, 16, 0), "1200", "13", "Unpresented Cheque" },
                new object[] { "12", "Steve", new DateTime(2013, 12, 31), new DateTime(2010, 4, 27, 17, 16, 0), "9998", "-13", "Unpresented Cheque" });

            var expected = new[]
            {
                new Transaction("26", new DateTimeOffset(2010,4,27,17,16, 0, TimeSpan.FromHours(1)), new DateTime(2013,12,31), "MANAGER",
                    "Unpresented Cheque", new[]
                    {
                        new LedgerEntry("1200", "Bank Current Account", JournalType.Dr, 55),
                        new LedgerEntry("9998", "Suspense Account", JournalType.Cr, 55),
                        new LedgerEntry("2200", "Sales Tax Control Account", JournalType.Dr, 0)
                    }),
                new Transaction("12", new DateTimeOffset(2010,4,27,17,16, 0, TimeSpan.FromHours(1)), new DateTime(2013,12,31), "Steve",
                    "Unpresented Cheque", new[]
                    {
                        new LedgerEntry("1200", "Bank Current Account", JournalType.Dr, 13),
                        new LedgerEntry("9998", "Suspense Account", JournalType.Cr, 13)
                    })
            };

            CollectionAssert.AreEqual(expected, journals);
        }      
        
        
        [Test]
        public void CanParseUnbalancedJournals()
        {
            var journals = ParseJournals(new object[] { "26", "MANAGER", new DateTime(2013, 12, 31), new DateTime(2010, 4, 27, 17, 16, 0), "1200", "55", "Unpresented Cheque" });

            var expected = new[]
            {
                new Transaction("26", new DateTimeOffset(2010,4,27,17,16,0, TimeSpan.FromHours(1)), new DateTime(2013, 12, 31), "MANAGER",
                    "Unpresented Cheque", new[]
                    {
                        new LedgerEntry("1200", "Bank Current Account", JournalType.Dr, 55)
                    })};

            CollectionAssert.AreEqual(expected, journals, "Sage parsing needs to be able to parse journals which don't balance, because for reasons known only to its devs, Sage supports them");
        }

        [Test]
        public void GetRightExceptionWhenUsernamesMismatched()
        {
            var exception = Assert.Throws<SqlDataFormatUnexpectedException>(() => ParseJournals(
                new object[] { "12", "Betty", new DateTime(2013, 12, 31), new DateTime(2010,4,27,17,16,0), "1200", "13", "Unpresented Cheque" },
                new object[] { "12", "Steve", new DateTime(2013, 12, 31), new DateTime(2010, 4, 27, 17, 16, 0), "1200", "13", "Unpresented Cheque" }));

            StringAssert.Contains("12", exception.Message, "If two fields conflict, user should be told what journal id is affected");
            foreach (var conflictingUsername in new[]{"Betty, Steve"})
            {
                StringAssert.Contains(conflictingUsername, exception.Message, "If two fields conflict, user should be told what the conflicting values are");
            }
        }

        [Test]
        public void GetFriendlyExceptionWhenNominalCodeNotDefined()
        {
            var exception = Assert.Throws<SqlDataFormatUnexpectedException>(() => ParseJournals(
                new object[] { "12", "Betty", new DateTime(2013, 12, 31), new DateTime(2010, 4, 27, 17, 16, 0), "bizarre nominal code", "13", "Unpresented Cheque" }));

            StringAssert.Contains("bizarre nominal code", exception.Message, "When a nominal code doesn't exist, the error message should tell you what code is causing the problem");
            foreach (var availableCode in nominalCodeLookup.Keys)
            {
                StringAssert.Contains(availableCode, exception.Message, "When a nominal code doesn't exist, the error message should let you know what nominal codes *were* defined");
            }
            
        }

        [Test]
        public void SchemaDefinitionIsValid()
        {
            var schema = new SageTransactionSchema();

            var definedColumnNumbers = schema.MappedColumns.Select(x => x.Index).ToList();
            var numberOfColumns = schema.MappedColumns.Count();

            var expectedDefinedColumnNumbers = Enumerable.Range(0, numberOfColumns).ToList();
            CollectionAssert.AreEqual(expectedDefinedColumnNumbers, definedColumnNumbers, "Column numbers should be consecutive, starting from 0, and schema should return them in order");
        }

        private static IEnumerable<Transaction> ParseJournals(params object[][] dataRows)
        {
            var reader = new SageTransactionReader(new SageTransactionSchema(), new SqlJournalReader(new JournalLineParser(), new TransactionCreator()));
            return reader.GetJournals(MockDataReader(dataRows), new NominalCodeLookup(nominalCodeLookup)).ToList();
        }       

        private static SqlDataReader MockDataReader(IEnumerable<object[]> rows)
        {
            var dataTable = new DataTable();
            dataTable.Columns.AddRange(GetSageColumns());
            foreach (var parameterArray in rows)
            {
                dataTable.Rows.Add(parameterArray);
            }
            return new SqlDataReader(dataTable.CreateDataReader());
        }

        private static DataColumn[] GetSageColumns()
        {
            return new SageTransactionSchema().MappedColumns.Select(x=>x.ToDataColumn()).ToArray();
        }
    }
}
