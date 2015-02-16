using System;
using System.Collections.Generic;
using CsvExport;
using Model;
using Model.Accounting;
using NUnit.Framework;
using Tests.Mocks;

namespace Tests
{
    [TestFixture]
    public class CsvExporterTests
    {
        private readonly List<Transaction> journals = new List<Transaction>
        {
            //one inside daylight savings
            new Transaction("id 1", new DateTimeOffset(new DateTime(2012, 3, 4), TimeSpan.Zero), new DateTime(2012, 3, 4), "alf",
                "very interesting journal", new List<LedgerEntry>
                {
                    new LedgerEntry("9012", "Expenses", LedgerEntryType.Cr, 23.4m),
                    new LedgerEntry("3001", "Cash", LedgerEntryType.Dr, 23.4m)
                }),

            //and one outside
            new Transaction("id 2", new DateTimeOffset(new DateTime(2012, 6, 5), TimeSpan.FromHours(1)),
                new DateTime(2012, 6, 5), "steve", "perfectly normal journal", new List<LedgerEntry>
                {
                    new LedgerEntry("8014", "Depreciation", LedgerEntryType.Cr, 12.4m),
                    new LedgerEntry("4001", "Fixed assets", LedgerEntryType.Dr, 12.4m)
                })
        };

        [Test]
        public void CanOutputAllFieldsSuccessfully()
        {
            var actual = GetExportedText("What we did to get these journals", journals, Enums.GetAllValues<DisplayField>());

            var expected =
@"What we did to get these journals
Created,Date,Username,Description
" + new DateTimeOffset(new DateTime(2012,3,4), TimeSpan.FromHours(0)) +","+ new DateTime(2012,3,4).ToShortDateString() + @",alf,very interesting journal,Cr,9012,Expenses,23.4,Dr,3001,Cash,23.4
" + new DateTimeOffset(new DateTime(2012,6,5), TimeSpan.FromHours(1)) +"," + new DateTime(2012,6,5).ToShortDateString() + @",steve,perfectly normal journal,Cr,8014,Depreciation,12.4,Dr,4001,Fixed assets,12.4
";
            
            Assert.AreEqual(expected, actual);
        }

        private static string GetExportedText(string description, IEnumerable<Transaction> journals, IEnumerable<DisplayField> fields)
        {
            var fileSystem = new MockFileSystem();
            var exporter = new CsvExporter(fileSystem);
            exporter.WriteJournals(description, journals, "c:\\steve.csv", fields);

            var actual = fileSystem.GetFileValue("c:\\steve.csv");
            return actual;
        }

        [Test]
        public void OnlyShowsSpecifiedFields()
        {
            var fileSystem = new MockFileSystem();
            var exporter = new CsvExporter(fileSystem);
            exporter.WriteJournals("An illuminating comment", journals, "c:\\steve.csv", new[]{DisplayField.JournalDate, DisplayField.Username,DisplayField.Amount, DisplayField.JournalType, DisplayField.AccountCode });

            var expected =
@"An illuminating comment
Date,Username
" + new DateTime(2012,3,4).ToShortDateString() + @",alf,Cr,9012,23.4,Dr,3001,23.4
" + new DateTime(2012,6,5).ToShortDateString() + @",steve,Cr,8014,12.4,Dr,4001,12.4
";
            Assert.AreEqual(expected, fileSystem.GetFileValue("c:\\steve.csv"));
        }
    }
}
