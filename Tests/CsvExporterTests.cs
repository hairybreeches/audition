using System;
using System.Collections.Generic;
using CsvExport;
using Model;
using Model.Accounting;
using NUnit.Framework;
using SqlImport;
using Tests.Mocks;

namespace Tests
{
    [TestFixture]
    public class CsvExporterTests
    {
        private readonly List<Transaction> transactions = new List<Transaction>
        {
            //one inside daylight savings
            new Transaction("id 1", new DateTime(2012, 3, 4), new DateTime(2012, 3, 4), "alf",
                "very interesting transaction", new List<LedgerEntry>
                {
                    new LedgerEntry("9012", "Expenses", LedgerEntryType.Cr, 23.4m),
                    new LedgerEntry("3001", "Cash", LedgerEntryType.Dr, 23.4m)
                }),

            //and one outside
            new Transaction("id 2", new DateTime(2012, 6, 5),
                new DateTime(2012, 6, 5), "steve", "perfectly normal transaction", new List<LedgerEntry>
                {
                    new LedgerEntry("8014", "Depreciation", LedgerEntryType.Cr, 12.4m),
                    new LedgerEntry("4001", "Fixed assets", LedgerEntryType.Dr, 12.4m)
                })
        };

        [Test]
        public void CanOutputAllFieldsSuccessfully()
        {
            var actual = GetExportedText("What we did to get these transactions", transactions, Enums.GetAllValues<DisplayField>());

            var expected =
@"What we did to get these transactions
Transaction ID,Entry time,Transaction date,Username,Description,Dr/Cr,Nominal Account,Account name,Amount
id 1," + new DateTime(2012, 3, 4) + "," + new DateTime(2012, 3, 4).ToShortDateString() + @",alf,very interesting transaction,Cr,9012,Expenses,23.4
id 1," + new DateTime(2012, 3, 4) + "," + new DateTime(2012, 3, 4).ToShortDateString() + @",alf,very interesting transaction,Dr,3001,Cash,23.4
id 2," + new DateTime(2012, 6, 5) + "," + new DateTime(2012, 6, 5).ToShortDateString() + @",steve,perfectly normal transaction,Cr,8014,Depreciation,12.4
id 2," + new DateTime(2012, 6, 5) + "," + new DateTime(2012, 6, 5).ToShortDateString() + @",steve,perfectly normal transaction,Dr,4001,Fixed assets,12.4
";
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void OnlyShowsSpecifiedFields()
        {
            var actual = GetExportedText("An illuminating comment", transactions, new[]{DisplayField.TransactionDate, DisplayField.Username,DisplayField.Amount, DisplayField.LedgerEntryType, DisplayField.AccountCode });

            var expected =
@"An illuminating comment
Transaction date,Username,Dr/Cr,Nominal Account,Amount
" + new DateTime(2012,3,4).ToShortDateString() + @",alf,Cr,9012,23.4
" + new DateTime(2012,3,4).ToShortDateString() + @",alf,Dr,3001,23.4
" + new DateTime(2012,6,5).ToShortDateString() + @",steve,Cr,8014,12.4
" + new DateTime(2012,6,5).ToShortDateString() + @",steve,Dr,4001,12.4
";
            Assert.AreEqual(expected, actual);
        }

        private static string GetExportedText(string description, IEnumerable<Transaction> transactions, IEnumerable<DisplayField> fields)
        {
            var fileSystem = new MockFileSystem();
            var exporter = new CsvExporter(fileSystem, new TabularFormatConverter());
            exporter.WriteTransactions(description, transactions, "c:\\steve.csv", fields);

            var actual = fileSystem.GetFileValue("c:\\steve.csv");
            return actual;
        }
    }
}
