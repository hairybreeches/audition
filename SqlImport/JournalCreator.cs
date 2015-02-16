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
        internal IEnumerable<Transaction> ReadJournals(IEnumerable<SqlJournalLine> lines)
        {
            var grouped = lines.GroupBy(x => x.TransactionId);
            return grouped.Select(CreateJournal);
        }

        private Transaction CreateJournal(IGrouping<string, SqlJournalLine> lines)
        {
            var journalLines = lines.ToList();
            return new Transaction(
                lines.Key,
                GetJournalField(journalLines, x => x.CreationTime, "creation time").ToUkDateTimeOffsetFromUkLocalTime(),
                GetJournalField(journalLines, x => x.JournalDate, "journal date"),
                GetJournalField(journalLines, x => x.Username, "username"),
                GetJournalField(journalLines, x => x.Description, "description"),
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
                throw new SqlDataFormatUnexpectedException(String.Format("Expected only one value for {0} per journal. Actual values for journal with id {1}: {2}. This can happen if you assign the 'ID' column incorrectly when importing data from Excel.", 
                    fieldName, journalLines.First().TransactionId, ValuesString(values)));
            }
            return values.Single();
        }

        private static string ValuesString<T>(IList<T> values)
        {
            const int maxListLength = 5;

            var valuesString = String.Join(", ", values.Take(maxListLength));
            if (values.Count > maxListLength)
            {
                valuesString += String.Format("and {0} more", values.Count - maxListLength);
            }
            return valuesString;
        }
    }
}