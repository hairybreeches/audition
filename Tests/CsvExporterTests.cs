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
        [Test]
        public void ShowsUsernameAndDescriptionIfSpecified()
        {
            var fileSystem = new MockFileSystem();
            var exporter = new CsvExporter(fileSystem);
            exporter.WriteJournals("What we did to get these journals", new List<Journal>
            {
                //one inside daylight savings
                new Journal("id 1", new DateTimeOffset(new DateTime(2012,3,4), TimeSpan.Zero),new DateTime(2012,3,4), "alf", "very interesting journal", new List<JournalLine>
                {
                    new JournalLine("9012", "Expenses", JournalType.Cr, 23.4m),
                    new JournalLine("3001", "Cash", JournalType.Dr, 23.4m)
                }  ),

                //and one outside
                new Journal("id 2", new DateTimeOffset(new DateTime(2012,6,5), TimeSpan.FromHours(1)),new DateTime(2012,6,5), "steve", "perfectly normal journal", new List<JournalLine>
                {
                    new JournalLine("8014", "Depreciation", JournalType.Cr, 12.4m),
                    new JournalLine("4001", "Fixed assets", JournalType.Dr, 12.4m)
                }  )
            }, "c:\\steve.csv", Enums.GetAllValues<DisplayField>());

            var expected =
@"What we did to get these journals
Created,Date,Username,Description
" + new DateTimeOffset(new DateTime(2012,3,4), TimeSpan.FromHours(0)) +","+ new DateTime(2012,3,4).ToShortDateString() + @",alf,very interesting journal,Cr,9012,Expenses,23.4,Dr,3001,Cash,23.4
" + new DateTimeOffset(new DateTime(2012,6,5), TimeSpan.FromHours(1)) +"," + new DateTime(2012,6,5).ToShortDateString() + @",steve,perfectly normal journal,Cr,8014,Depreciation,12.4,Dr,4001,Fixed assets,12.4
";
            Assert.AreEqual(expected, fileSystem.GetFileValue("c:\\steve.csv"));
        }
    }
}
