using System;

namespace CsvExport
{
    public interface ISpreadsheetWriter : IDisposable
    {
        void WriteField<T>(T field);
        void NextRecord();
    }
}