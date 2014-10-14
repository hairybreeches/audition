using System.Collections.Generic;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;
using Model;
using Model.Accounting;

namespace Excel
{
    public class ExcelExporter
    {
        private readonly IFileSystem fileSystem;

        public ExcelExporter(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void WriteJournals(IEnumerable<Journal> journals, string filename, SerialisationOptions options)
        {
            using (var writer = CreateWriter(filename))
            {
                WriteHeaderRow(writer, options);
                foreach (var journal in journals)
                {
                    WriteJournal(writer, journal, options);
                }                                
            }
        }

        private void WriteHeaderRow(CsvWriter writer, SerialisationOptions options)
        {
            writer.WriteField("Created");         
            writer.WriteField("Date");
            if (options.ShowUsername)
            {
                writer.WriteField("Username");
            }
            
            if (options.ShowDescription)
            {
                writer.WriteField("Description");
            }


            writer.NextRecord();
        }

        private void WriteJournal(CsvWriter writer, Journal journal, SerialisationOptions options)
        {
            writer.WriteField(journal.Created);
            writer.WriteField(journal.JournalDate);
            if (options.ShowUsername)
            {
                writer.WriteField(journal.Username);
            }
            
            if (options.ShowDescription)
            {
                writer.WriteField(journal.Description);
            }

            foreach (var line in journal.Lines)
            {
                WriteLine(writer, line);
            }
            writer.NextRecord();
        }

        private static void WriteLine(CsvWriter writer, JournalLine line)
        {
            writer.WriteField(line.JournalType);
            writer.WriteField(line.AccountCode);
            writer.WriteField(line.AccountName);
            writer.WriteField(line.Amount);
        }

        private CsvWriter CreateWriter(string filename)
        {
            return new CsvWriter(fileSystem.OpenFileToWrite(filename));
        }        
    }    
}
