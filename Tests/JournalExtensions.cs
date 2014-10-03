using System.Linq;
using Model;
using Model.Accounting;
using XeroApi.Model;
using Journal = Model.Accounting.Journal;
using JournalLine = XeroApi.Model.JournalLine;

namespace Tests
{
    static class JournalExtensions
    {
        public static XeroApi.Model.Journal ToXeroJournal(this Journal modelJournal)
        {
            var lines = new JournalLines();
            lines.AddRange(modelJournal.Lines.Select(ToXeroJournalLine));

            return new XeroApi.Model.Journal
            {
                JournalID = modelJournal.Id,
                CreatedDateUTC = modelJournal.Created.UtcDateTime,
                JournalDate = modelJournal.JournalDate,
                JournalLines = lines
            };
        }

        private static JournalLine ToXeroJournalLine(Model.Accounting.JournalLine line)
        {
            return new JournalLine
            {
                AccountCode = line.AccountCode,
                AccountName = line.AccountName,
                NetAmount = line.JournalType == JournalType.Cr ? line.Amount*-1 : line.Amount
            };
        }
    }
}
