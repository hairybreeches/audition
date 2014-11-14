using System.Collections.Generic;
using System.Linq;
using Excel;
using Model;
using Model.Accounting;

namespace Tests.Mocks
{
    public class MockExporter : IExcelExporter
    {
        public IEnumerable<Journal> WrittenJournals { get; private set; }

        public void WriteJournals(string description, IEnumerable<Journal> journals, string filename, SerialisationOptions options)
        {
            //evaluate the IEnumerable here in case things go out of scope when we want to access the data
            WrittenJournals = journals.ToList();
        }
    }
}