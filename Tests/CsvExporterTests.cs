using System;
using System.Collections.Generic;
using Autofac;
using Capabilities;
using CsvExport;
using Model;
using Model.Accounting;
using Native.Disk;
using NUnit.Framework;
using SqlImport;
using Tests.Mocks;
using Webapp;

namespace Tests
{
    [TestFixture]
    public class CsvExporterTests
    {
        private readonly List<Transaction> transactions = new List<Transaction>
        {
            //one inside daylight savings
            new Transaction("id 1", new DateTime(2012, 3, 4), "alf",
                "very interesting transaction", new List<LedgerEntry>
                {
                    new LedgerEntry("9012", "Expenses", LedgerEntryType.Cr, 23.4m),
                    new LedgerEntry("3001", "Cash", LedgerEntryType.Dr, 23.4m)
                }, "SI"),

            //and one outside
            new Transaction("id 2", 
                new DateTime(2012, 6, 5), "steve", "perfectly normal transaction", new List<LedgerEntry>
                {
                    new LedgerEntry("8014", "Depreciation", LedgerEntryType.Cr, 12.4m),
                    new LedgerEntry("4001", "Fixed assets", LedgerEntryType.Dr, 12.4m)
                }, "UJ")
        };

        [Test]
        public void CanOutputAllFieldsSuccessfully()
        {
            var actual = GetExportedText("What we did to get these transactions", transactions, Enums.GetAllValues<DisplayField>());

            var expected =
@"What we did to get these transactions
Transaction ID,Transaction date,Transaction type,Username,Description,Dr/Cr,Nominal Account,Account name,Amount
id 1,2012-03-04,SI,alf,very interesting transaction,Cr,9012,Expenses,23.4
id 1,2012-03-04,SI,alf,very interesting transaction,Dr,3001,Cash,23.4
id 2,2012-06-05,UJ,steve,perfectly normal transaction,Cr,8014,Depreciation,12.4
id 2,2012-06-05,UJ,steve,perfectly normal transaction,Dr,4001,Fixed assets,12.4
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
2012-03-04,alf,Cr,9012,23.4
2012-03-04,alf,Dr,3001,23.4
2012-06-05,steve,Cr,8014,12.4
2012-06-05,steve,Dr,4001,12.4
";
            Assert.AreEqual(expected, actual);
        }

        private static string GetExportedText(string description, IEnumerable<Transaction> transactions, ICollection<DisplayField> fields)
        {
            var fileSystem = new MockFileSystem();
            using (var lifetime = GetLifetime(fileSystem))
            {
                var exporter = lifetime.Resolve<CsvExporter>();
                var filename = "c:\\steve.csv";
                exporter.Export(description, transactions, filename, fields);

                var actual = fileSystem.GetFileValue(filename);
                return actual;
            }                        
        }

        private static IContainer GetLifetime(MockFileSystem fileSystem)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<WebappModule>();
            builder.Register(_ => fileSystem).As<IFileSystem>();
            return builder.Build();
        }
    }
}
