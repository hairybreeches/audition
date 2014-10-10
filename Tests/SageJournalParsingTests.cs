using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using Model.Accounting;
using NUnit.Framework;
using Sage50;
using Sage50.Parsing;

namespace Tests
{
    [TestFixture]
    public class SageJournalParsingTests
    {
        [Test]
        public void CanConvertJournals()
        {
            var journals = ParseJournals(
                new[]
                {
                    "26", "MANAGER", "31/12/2013", "27/04/2010 17:16", "1200", "55", "Unpresented Cheque"
                },
                new[]
                {
                    "26", "MANAGER", "31/12/2013", "27/04/2010 17:16", "9998", "-55", "Unpresented Cheque"
                },
                new[]
                {
                    "26", "MANAGER", "31/12/2013", "27/04/2010 17:16", "2200", "0", "Unpresented Cheque"
                },
                new[]
                {
                    "12", "Steve", "31/12/2013", "27/04/2010 17:16", "1200", "13", "Unpresented Cheque"
                },
                new[]
                {
                    "12", "Steve", "31/12/2013", "27/04/2010 17:16", "9998", "-13", "Unpresented Cheque"
                });

            var expected = new[]
            {
                new Journal("26", DateTime.Parse("27/04/2010 17:16"), DateTime.Parse("31/12/2013"), "MANAGER",
                    "Unpresented Cheque", new[]
                    {
                        new JournalLine("1200", "1200", JournalType.Dr, 55),
                        new JournalLine("9998", "9998", JournalType.Cr, 55),
                        new JournalLine("2200", "2200", JournalType.Dr, 0)
                    }),
                new Journal("12", DateTime.Parse("27/04/2010 17:16"), DateTime.Parse("31/12/2013"), "Steve",
                    "Unpresented Cheque", new[]
                    {
                        new JournalLine("1200", "1200", JournalType.Dr, 13),
                        new JournalLine("9998", "9998", JournalType.Cr, 13),
                    })
            };

            CollectionAssert.AreEqual(expected, journals);
        }

        private static IEnumerable<Journal> ParseJournals(params string[][] dataRows)
        {
            var reader = new JournalReader(MockDataReader(dataRows), new JournalLineParser(new JournalSchema()));
            return reader.GetJournals().ToList();
        }       


        [Test]
        public void SchemaDefinitionIsValid()
        {
            var schema = new JournalSchema();

            var definedColumnNumbers = schema.Columns.Select(x => x.Index).ToList();
            var numberOfColumns = schema.Columns.Count();

            var expectedDefinedColumnNumbers = Enumerable.Range(0, numberOfColumns).ToList();
            CollectionAssert.AreEqual(expectedDefinedColumnNumbers, definedColumnNumbers, "Column numbers should be consecutive, starting from 0, and schema should return them in order");
        }

        private static IDataReader MockDataReader(IEnumerable<object[]> rows)
        {
            var dataTable = new DataTable();
            dataTable.Columns.AddRange(GetSageColumns());
            foreach (var parameterArray in rows)
            {
                dataTable.Rows.Add(parameterArray);
            }
                

            return dataTable.CreateDataReader();
        }

        private static DataColumn[] GetSageColumns()
        {
            return new JournalSchema().Columns.Select(x=>x.ToDataColumn()).ToArray();
        }
    }
}
