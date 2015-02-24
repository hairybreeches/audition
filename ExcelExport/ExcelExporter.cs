using System.Collections.Generic;
using System.Linq;
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
        private readonly IEnumerable<IFormatterFactory> formatterFactories;

        public ExcelExporter(CsvExporter csvExporter, IFileSystem fileSystem, ExcelFileOpener fileOpener, IEnumerable<IFormatterFactory> formatterFactories)
        {
            this.csvExporter = csvExporter;
            this.fileSystem = fileSystem;
            this.fileOpener = fileOpener;
            this.formatterFactories = formatterFactories;
        }

        public void Export(string description, IEnumerable<Transaction> transactions, string filename, IEnumerable<DisplayField> availableFields)
        {
            using (var tempFile = fileSystem.GetTempFile("csv"))
            {
                csvExporter.Export(description, transactions, tempFile.Filename, availableFields);
                using (var excelWriter = fileOpener.OpenFile(tempFile.Filename))
                {
                    excelWriter.MergeRow(1);
                    var displayFields = new HashSet<DisplayField>(availableFields);
                    excelWriter.FormatColumns(formatterFactories.Select(x => x.GetFormatter(displayFields)), 2);
                    excelWriter.ApplyFiltersToRow(2);                    
                    excelWriter.AutosizeColumns();
                    excelWriter.NameSheet("Audition search");
                    excelWriter.SaveAs(filename);
                }
            }
        }
    }
}