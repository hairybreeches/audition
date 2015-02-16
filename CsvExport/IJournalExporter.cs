using System.Collections.Generic;
using Model;
using Model.Accounting;

namespace CsvExport
{
    public interface IJournalExporter
    {
        void WriteJournals(string description, IEnumerable<Transaction> journals, string filename, IEnumerable<DisplayField> availableFields);
    }
}