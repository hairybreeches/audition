using System.Collections.Generic;
using System.Linq;
using Capabilities;
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

        public void Export(string description, IEnumerable<Transaction> transactions, string filename, ICollection<DisplayField> availableFields)
        {
            using (var tempFile = fileSystem.GetTempFile("csv"))
            {
                csvExporter.Export(description, transactions, tempFile.Filename, availableFields);
                using (var excelWriter = fileOpener.OpenFile(tempFile.Filename))
                {
                    excelWriter.MergeRow(1);                    
                    excelWriter.FormatColumns(formatterFactories.Select(x => x.GetFormatter(availableFields)), 2);
                    excelWriter.ApplyFiltersToRow(2);                    
                    excelWriter.AutosizeColumns();
                    excelWriter.NameSheet("Audition search");
                    excelWriter.SaveAs(filename);
                }
            }
        }
    }
}