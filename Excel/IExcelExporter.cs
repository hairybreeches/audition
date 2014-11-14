using System.Collections.Generic;
using Model;
using Model.Accounting;

namespace Excel
{
    public interface IExcelExporter
    {
        void WriteJournals(string description, IEnumerable<Journal> journals, string filename, SerialisationOptions options);
    }
}