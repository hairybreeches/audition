using System;
using System.Collections.Generic;
using System.Linq;
using Model.Accounting;

namespace Sage50.Parsing
{
    /// <summary>
    /// Knows how to turn intermediate parsing step SageJournalLine into Journals.
    /// Don't use this directly, use a JournalReader.
    /// </summary>
    static class JournalParsing
    {
        public static IEnumerable<Journal> ReadJournals(IEnumerable<SageJournalLine> lines)
        {
            var grouped = lines.GroupBy(x => x.TransactionId);
            return grouped.Select(CreateJournal);
        }

        private static Journal CreateJournal(IEnumerable<SageJournalLine> linesEnumerable)
        {
            var journalLines = linesEnumerable.ToList();
            return new Journal(
                GetJournalField(journalLines, x => x.TransactionId).ToString(),
                new DateTimeOffset(GetJournalField(journalLines, x => x.CreationTime)),
                GetJournalField(journalLines, x => x.JournalDate),
                GetJournalField(journalLines, x => x.Username),
                GetJournalField(journalLines, x => x.Description),
                journalLines.Select(ToModelLine));


        }

        private static JournalLine ToModelLine(SageJournalLine arg)
        {
            return new JournalLine(arg.NominalCode, arg.NominalCodeName, arg.JournalType, arg.Amount);
        }

        private static T GetJournalField<T>(IList<SageJournalLine> journalLines, Func<SageJournalLine, T> getter)
        {
            var values = journalLines.Select(getter).Distinct().ToList();
            if (values.Count > 1)
            {
                throw new SageDataFormatUnexpectedException(String.Format("Expected only one value for property per transaction. Actual values for journal {0}: {1}", 
                    journalLines.First().TransactionId, String.Join(", ", values)));
            }
            return values.Single();
        }
    }
}