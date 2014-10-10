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
        public void CanConvertJournal()
        {
            var reader = new JournalReader(MockDataReader(), new JournalLineParser(new JournalSchema()));
            var journals = reader.GetJournals().ToList();
            CollectionAssert.AreEqual(new[] { new Journal("26", DateTime.Parse("27/04/2010 17:16"), DateTime.Parse("31/12/2013"), "MANAGER", "Unpresented Cheque", new []
            {
                new JournalLine("1200", "1200", JournalType.Dr, 55), 
                new JournalLine("9998", "9998", JournalType.Cr, 55), 
                new JournalLine("2200", "2200", JournalType.Dr, 0)
            })}, journals);
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

        public IDataReader MockDataReader()
        {
            var dataTable = new DataTable();
            dataTable.Columns.AddRange(GetSageColumns());
            foreach (var parameterArray in GetRows())
            {
                dataTable.Rows.Add(parameterArray);
            }
                

            return dataTable.CreateDataReader();
        }

        private static IEnumerable<object[]> GetRows()
        {
            return new[]
            {
                new[]
                {
                    "26", "MANAGER", "31/12/2013","27/04/2010 17:16","1200", "55", "Unpresented Cheque"
                },
                new[]
                {
                    "26", "MANAGER", "31/12/2013","27/04/2010 17:16", "9998","-55", "Unpresented Cheque"
                },
                new[]
                {
                    "26", "MANAGER", "31/12/2013","27/04/2010 17:16", "2200","0", "Unpresented Cheque"
                }
            };
        }

        private DataColumn[] GetSageColumns()
        {
            return new JournalSchema().Columns.Select(x=>x.ToDataColumn()).ToArray();
        }
    }
}
