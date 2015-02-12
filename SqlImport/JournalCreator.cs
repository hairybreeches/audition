using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Model.Accounting;

namespace SqlImport
{
    /// <summary>
    /// Knows how to turn intermediate parsing step SqlJournalLine into Journals.
    /// Don't use this directly, use a JournalReader.
    /// </summary>
    public class JournalCreator
    {
        internal IEnumerable<Journal> ReadJournals(IEnumerable<SqlJournalLine> lines)
        {
            var grouped = lines.GroupBy(x => x.TransactionId);
            return grouped.Select(CreateJournal);
        }

        private Journal CreateJournal(IGrouping<string, SqlJournalLine> lines)
        {
            var journalLines = lines.ToList();
            return new Journal(
                lines.Key,
                GetJournalField(journalLines, x => x.CreationTime, "Creation time").ToUkDateTimeOffsetFromUkLocalTime(),
                GetJournalField(journalLines, x => x.JournalDate, "Journal date"),
                GetJournalField(journalLines, x => x.Username, "Username"),
                GetJournalField(journalLines, x => x.Description, "Description"),
                journalLines.Select(ToModelLine));


        }

        private static JournalLine ToModelLine(SqlJournalLine arg)
        {
            return new JournalLine(arg.NominalCode, arg.NominalCodeName, arg.JournalType, arg.Amount);
        }

        private static T GetJournalField<T>(IList<SqlJournalLine> journalLines, Func<SqlJournalLine, T> getter, string fieldName)
        {
            var values = journalLines.Select(getter).Distinct().ToList();
            if (values.Count > 1)
            {
                throw new SqlDataFormatUnexpectedException(String.Format("Expected only one value for {0} per transaction. Actual values for journal {1}: {2}", 
                    fieldName, journalLines.First().TransactionId, String.Join(", ", values)));
            }
            return values.Single();
        }
    }
}