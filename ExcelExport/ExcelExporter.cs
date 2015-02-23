using System.Collections.Generic;
using CsvExport;
using Model;
using Model.Accounting;
using Native.Disk;

namespace ExcelExport
{
    class ExcelExporter : ITransactionExporter
    {
        private readonly CsvExporter csvExporter;
        private readonly IFileSystem fileSystem;
        private readonly ExcelFileOpener fileOpener;

        public ExcelExporter(CsvExporter csvExporter, IFileSystem fileSystem, ExcelFileOpener fileOpener)
        {
            this.csvExporter = csvExporter;
            this.fileSystem = fileSystem;
            this.fileOpener = fileOpener;
        }

        public void Export(string description, IEnumerable<Transaction> transactions, string filename, IEnumerable<DisplayField> availableFields)
        {
            using (var tempFile = fileSystem.GetTempFile("csv"))
            {
                csvExporter.Export(description, transactions, tempFile.Filename, availableFields);
                using (var excelWriter = fileOpener.OpenFile(tempFile.Filename))
                {
                    excelWriter.MergeRow(1);
                    excelWriter.ApplyFiltersToRow(2);
                    excelWriter.AutosizeColumns();
                    excelWriter.NameSheet("Audition search");
                    excelWriter.SaveAs(filename);
                }
            }
        }
    }
}