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
        public IEnumerable<Journal> ReadJournals(IEnumerable<SqlJournalLine> lines)
        {
            var grouped = lines.GroupBy(x => x.TransactionId);
            return grouped.Select(CreateJournal);
        }

        private Journal CreateJournal(IEnumerable<SqlJournalLine> linesEnumerable)
        {
            var journalLines = linesEnumerable.ToList();
            return new Journal(
                GetJournalField(journalLines, x => x.TransactionId),
                GetJournalField(journalLines, x => x.CreationTime).ToUkDateTimeOffsetFromUkLocalTime(),
                GetJournalField(journalLines, x => x.JournalDate),
                GetJournalField(journalLines, x => x.Username),
                GetJournalField(journalLines, x => x.Description),
                journalLines.Select(ToModelLine));


        }

        private static JournalLine ToModelLine(SqlJournalLine arg)
        {
            return new JournalLine(arg.NominalCode, arg.NominalCodeName, arg.JournalType, arg.Amount);
        }

        private static T GetJournalField<T>(IList<SqlJournalLine> journalLines, Func<SqlJournalLine, T> getter)
        {
            var values = journalLines.Select(getter).Distinct().ToList();
            if (values.Count > 1)
            {
                throw new SqlDataFormatUnexpectedException(String.Format("Expected only one value for property per transaction. Actual values for journal {0}: {1}", 
                    journalLines.First().TransactionId, String.Join(", ", values)));
            }
            return values.Single();
        }
    }
}