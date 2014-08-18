using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Excel;
using Model;
using NUnit.Framework;

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
                new Journal(Guid.NewGuid(), new DateTime(2012,3,4),new DateTime(2012,3,4), new List<JournalLine>
                {
                    new JournalLine("9012", "Expenses", JournalType.Cr, 23.4m),
                    new JournalLine("3001", "Cash", JournalType.Dr, 23.4m)
                }  ),
                new Journal(Guid.NewGuid(), new DateTime(2012,6,5),new DateTime(2012,6,5), new List<JournalLine>
                {
                    new JournalLine("9012", "Expenses", JournalType.Cr, 12.4m),
                    new JournalLine("3001", "Cash", JournalType.Dr, 12.4m)
                }  )
            }, "c:\\steve.csv"
            );

            var expected = 
@"Created,Date
04/03/2012 00:00:00,04/03/2012 00:00:00
05/06/2012 00:00:00,05/06/2012 00:00:00
";
            Assert.AreEqual(expected, fileSystem.GetFileValue("c:\\steve.csv"));
        }
    }
}
