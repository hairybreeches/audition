﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Model;
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
        public void CanConvertTransactions()
        {
            var transactions = ParseTransactions(
                new object[] { "26", "MANAGER", new DateTime(2013, 12, 31), "1200", "55", "Unpresented Cheque", "CI" },
                new object[] { "26", "MANAGER", new DateTime(2013, 12, 31), "9998", "-55", "Unpresented Cheque", "CI" },
                new object[] { "26", "MANAGER", new DateTime(2013, 12, 31), "2200", "0", "Unpresented Cheque", "CI" },
                new object[] { "12", "Steve", new DateTime(2013, 12, 31), "1200", "13", "Unpresented Cheque", "UJ" },
                new object[] { "12", "Steve", new DateTime(2013, 12, 31), "9998", "-13", "Unpresented Cheque", "UJ" });

            var expected = new[]
            {
                new Transaction("26", new DateTime(2013,12,31), "MANAGER",
                    "Unpresented Cheque", "CI", 
                        new LedgerEntry("1200", "Bank Current Account", LedgerEntryType.Dr, 55),
                        new LedgerEntry("9998", "Suspense Account", LedgerEntryType.Cr, 55),
                        new LedgerEntry("2200", "Sales Tax Control Account", LedgerEntryType.Dr, 0)
                    ),
                new Transaction("12", new DateTime(2013,12,31), "Steve",
                    "Unpresented Cheque", "UJ", 
                        new LedgerEntry("1200", "Bank Current Account", LedgerEntryType.Dr, 13),
                        new LedgerEntry("9998", "Suspense Account", LedgerEntryType.Cr, 13))
            };

            CollectionAssert.AreEqual(expected, transactions);
        }    
        
        [Test]
        public void IfOptionalValuesNullThenDefaultIsProvided()
        {
            var transactions = ParseTransactions(
                new object[] { "26", DBNull.Value, new DateTime(2013, 12, 31), "1200", "55", DBNull.Value, "CI" },
                new object[] { "26", DBNull.Value, new DateTime(2013, 12, 31), "9998", "-55", DBNull.Value, "CI" },
                new object[] { "26", DBNull.Value, new DateTime(2013, 12, 31), "2200", "0", DBNull.Value, "CI" },
                new object[] { "12", DBNull.Value, new DateTime(2013, 12, 31), "1200", "13", DBNull.Value, "UJ" },
                new object[] { "12", DBNull.Value, new DateTime(2013, 12, 31), "9998", "-13", DBNull.Value, "UJ" });

            var expected = new[]
            {
                new Transaction("26", new DateTime(2013,12,31), "<none>",
                    "<none>", "CI", 
                        new LedgerEntry("1200", "Bank Current Account", LedgerEntryType.Dr, 55),
                        new LedgerEntry("9998", "Suspense Account", LedgerEntryType.Cr, 55),
                        new LedgerEntry("2200", "Sales Tax Control Account", LedgerEntryType.Dr, 0)
                    ),
                new Transaction("12", new DateTime(2013,12,31), "<none>",
                    "<none>", "UJ",
                        new LedgerEntry("1200", "Bank Current Account", LedgerEntryType.Dr, 13),
                        new LedgerEntry("9998", "Suspense Account", LedgerEntryType.Cr, 13))
            };

            CollectionAssert.AreEqual(expected, transactions);
        }      
        
        
        [Test]
        public void CanParseUnbalancedTransactions()
        {
            var transactions = ParseTransactions(new object[] { "26", "MANAGER", new DateTime(2013, 12, 31), "1200", "55", "Unpresented Cheque", "AF" });

            var expected = new[]
            {
                new Transaction("26", new DateTime(2013, 12, 31), "MANAGER",
                    "Unpresented Cheque", "AF", 
                        new LedgerEntry("1200", "Bank Current Account", LedgerEntryType.Dr, 55)
                    )};

            CollectionAssert.AreEqual(expected, transactions, "Sage parsing needs to be able to parse transactions which don't balance, because for reasons known only to its devs, Sage supports them");
        }

        [Test]
        public void GetRightExceptionWhenUsernamesMismatched()
        {
            var exception = Assert.Throws<SqlDataFormatUnexpectedException>(() => ParseTransactions(
                new object[] { "12", "Betty", new DateTime(2013, 12, 31), "1200", "13", "Unpresented Cheque" },
                new object[] { "12", "Steve", new DateTime(2013, 12, 31), "1200", "13", "Unpresented Cheque" }));

            StringAssert.Contains("12", exception.Message, "If two fields conflict, user should be told what transaction id is affected");
            foreach (var conflictingUsername in new[]{"Betty, Steve"})
            {
                StringAssert.Contains(conflictingUsername, exception.Message, "If two fields conflict, user should be told what the conflicting values are");
            }
        }

        [Test]
        public void GetDefaultValueWhenNominalCodeNotDefined()
        {
            var transaction = ParseTransactions(
                new object[]
                {"12", "Betty", new DateTime(2013, 12, 31), "bizarre nominal code", "13", "Unpresented Cheque", "JC"})
                .Single();
            Assert.AreEqual(new Transaction("12", new DateTime(2013,12,31),"Betty", "Unpresented Cheque", "Journal Credit", new LedgerEntry("bizarre nominal code", "<none>", LedgerEntryType.Dr, 13)), transaction);
            
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

        private static IEnumerable<Transaction> ParseTransactions(params object[][] dataRows)
        {
            var reader = new SageTransactionReader(new SageTransactionSchema(), new SqlFinancialTransactionReader(new LedgerEntryParser(), new TabularFormatConverter()));
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
