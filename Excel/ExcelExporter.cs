using System.Collections.Generic;
using CsvHelper;
using Model;
using Model.Accounting;
using Native;

namespace Excel
{
    public class ExcelExporter : IExcelExporter
    {
        private readonly IFileSystem fileSystem;

        public ExcelExporter(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void WriteJournals(string description, IEnumerable<Journal> journals, string filename, SerialisationOptions options)
        {
            using (var writer = CreateWriter(filename))
            {
                WriteDescriptionRow(writer, description);
                WriteHeaderRow(writer, options);
                foreach (var journal in journals)
                {
                    WriteJournal(writer, journal, options);
                }                                
            }
        }

        private static void WriteDescriptionRow(ICsvWriter writer, string description)
        {
            writer.WriteField(description);
            writer.NextRecord();
        }

        private static void WriteHeaderRow(ICsvWriter writer, SerialisationOptions options)
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

        private static void WriteJournal(ICsvWriter writer, Journal journal, SerialisationOptions options)
        {
            writer.WriteField(journal.Created);
            writer.WriteField(journal.JournalDate.ToShortDateString());
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

        private static void WriteLine(ICsvWriter writer, JournalLine line)
        {
            writer.WriteField(line.JournalType);
            writer.WriteField(line.AccountCode);
            writer.WriteField(line.AccountName);
            writer.WriteField(line.Amount);
        }

        private ICsvWriter CreateWriter(string filename)
        {
            return new CsvWriter(fileSystem.OpenFileToWrite(filename));
        }        
    }    
}
