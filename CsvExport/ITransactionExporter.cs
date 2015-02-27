using System.Collections.Generic;
using Capabilities;
using Model;
using Model.Accounting;

namespace CsvExport
{
    public interface ITransactionExporter
    {
        void Export(string description, IEnumerable<Transaction> transactions, string filename, ICollection<DisplayField> availableFields);
    }
}