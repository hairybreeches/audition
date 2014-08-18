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
                WriteHeaderRow(writer);
                foreach (var journal in journals)
                {
                    WriteJournal(writer, journal);
                }                                
            }
        }

        private void WriteHeaderRow(CsvWriter writer)
        {
            writer.WriteField("Created");         
            writer.WriteField("Date");
            writer.NextRecord();
        }

        private void WriteJournal(CsvWriter writer, Journal journal)
        {
            writer.WriteField(journal.Created);
            writer.WriteField(journal.JournalDate);   
            writer.NextRecord();
        }

        private CsvWriter CreateWriter(string filename)
        {
            return new CsvWriter(fileSystem.OpenFileToWrite(filename));
        }        
    }    
}
