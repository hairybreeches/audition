using System.Collections.Generic;
using System.Linq;
using CsvExport;
using Model.Accounting;

namespace Tests.Mocks
{
    public class MockExporter : IJournalExporter
    {
        public IEnumerable<Journal> WrittenJournals { get; private set; }

        public void WriteJournals(string description, IEnumerable<Journal> journals, string filename)
        {
            //evaluate the IEnumerable here in case things go out of scope when we want to access the data
            WrittenJournals = journals.ToList();
        }
    }
}