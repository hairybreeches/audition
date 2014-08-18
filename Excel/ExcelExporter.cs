using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Model;

namespace Excel
{
    public class ExcelExporter
    {
        private readonly IFileSystem fileSystem;

        public ExcelExporter(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void WriteJournals(IEnumerable<Journal> journals, string filename)
        {
            using (var writer = CreateWriter(filename))
            {
                writer.WriteRecords(journals);
                
            }
        }

        private CsvWriter CreateWriter(string filename)
        {
            var csvWriter = new CsvWriter(fileSystem.OpenFileToWrite(filename));
            csvWriter.Configuration.RegisterClassMap<JournalMap>();
            return csvWriter;
        }        
    }

    public class JournalMap : CsvClassMap<Journal>
    {
        public JournalMap()
        {
            Map(m => m.Created).Name("Created");
            Map(m => m.JournalDate).Name("Date");
        }
    }
}
