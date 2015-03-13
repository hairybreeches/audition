using System.Collections.Generic;
using System.Linq;
using Capabilities;
using CsvExport;
using Model;
using Model.Accounting;

namespace Tests.Mocks
{
    public class MockExporter : ITransactionExporter
    {
        public IEnumerable<Transaction> WrittenTransactions { get; private set; }

        public void Export(string description, IEnumerable<Transaction> transactions, string filename, IList<DisplayField> availableFields)
        {
            //evaluate the IEnumerable here in case things go out of scope when we want to access the data
            WrittenTransactions = transactions.ToList();
        }
    }
}