using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Excel;
using Model;
using Model.Accounting;
using NUnit.Framework;
using Tests.Mocks;

namespace Tests
{
    [TestFixture]
    public class ExcelExporterTests
    {
        [Test]
        public void ExportsJournalsCorrectly()
        {
            var fileSystem = new MockFileSystem();
            var exporter = new ExcelExporter(fileSystem);
            exporter.WriteJournals(new List<Journal>
            {
                //one inside daylight savings
                new Journal(Guid.NewGuid(), new DateTimeOffset(new DateTime(2012,3,4), TimeSpan.Zero),new DateTime(2012,3,4), new List<JournalLine>
                {
                    new JournalLine("9012", "Expenses", JournalType.Cr, 23.4m),
                    new JournalLine("3001", "Cash", JournalType.Dr, 23.4m)
                }  ),

                //and one outside
                new Journal(Guid.NewGuid(), new DateTimeOffset(new DateTime(2012,6,5), TimeSpan.FromHours(1)),new DateTime(2012,6,5), new List<JournalLine>
                {
                    new JournalLine("8014", "Depreciation", JournalType.Cr, 12.4m),
                    new JournalLine("4001", "Fixed assets", JournalType.Dr, 12.4m)
                }  )
            }, "c:\\steve.csv"
            );

            var expected =
@"Created,Date
04/03/2012 00:00:00 +00:00,04/03/2012 00:00:00,Cr,9012,Expenses,23.4,Dr,3001,Cash,23.4
05/06/2012 00:00:00 +01:00,05/06/2012 00:00:00,Cr,8014,Depreciation,12.4,Dr,4001,Fixed assets,12.4
";
            Assert.AreEqual(expected, fileSystem.GetFileValue("c:\\steve.csv"));
        }
    }
}
