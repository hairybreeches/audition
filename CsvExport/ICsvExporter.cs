using System.Collections.Generic;
using Model;
using Model.Accounting;

namespace CsvExport
{
    public interface ICsvExporter
    {
        void WriteJournals(string description, IEnumerable<Journal> journals, string filename, SerialisationOptions options);
    }
}