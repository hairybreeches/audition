using System.Collections.Generic;
using CsvHelper;
using Model;
using Model.Accounting;
using Native;

namespace CsvExport
{
    public class CsvExporter : IJournalExporter
    {
        private readonly IFileSystem fileSystem;

        public CsvExporter(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public void WriteJournals(string description, IEnumerable<Journal> journals, string filename, IEnumerable<DisplayField> availableFields)
        {
            using (var writer = CreateWriter(filename))
            {
                WriteDescriptionRow(writer, description);
                WriteHeaderRow(writer);
                foreach (var journal in journals)
                {
                    WriteJournal(writer, journal);
                }                                
            }
        }

        private static void WriteDescriptionRow(ICsvWriter writer, string description)
        {
            writer.WriteField(description);
            writer.NextRecord();
        }

        private static void WriteHeaderRow(ICsvWriter writer)
        {
            writer.WriteField("Created");
            writer.WriteField("Date");
            writer.WriteField("Username");
            writer.WriteField("Description");
            writer.NextRecord();
        }

        private static void WriteJournal(ICsvWriter writer, Journal journal)
        {
            writer.WriteField(journal.Created);
            writer.WriteField(journal.JournalDate.ToShortDateString());
            writer.WriteField(journal.Username);
            writer.WriteField(journal.Description);

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
