using System;
using System.Linq;
using Model;
using Model.Accounting;
using Journal = XeroApi.Model.Journal;
using JournalLine = XeroApi.Model.JournalLine;

namespace Xero
{
    public static class XeroJournalExtensions
    {
        public static Model.Accounting.Journal ToModelJournal(this Journal xeroJournal)
        {
            var modelJournal = new Model.Accounting.Journal(xeroJournal.JournalID, xeroJournal.UkCreationTime(), xeroJournal.JournalDate, xeroJournal.JournalLines.Select(ToModelJournalLine));
            ValidateLines(modelJournal);
            return modelJournal;
        }

        private static void ValidateLines(Model.Accounting.Journal journal)
        {
            var sum = journal.Lines.Select(GetLineAmount).Sum();

            if (sum != 0)
            {
                throw new InvalidJournalException(String.Format("Lines for journal {0} do not balance: {1}", journal.Id, String.Join(",", journal.Lines.Select(x => x.ToString()))));
            }
        }

        private static decimal GetLineAmount(Model.Accounting.JournalLine line)
        {
            return line.JournalType == Model.Accounting.JournalType.Cr ? line.Amount * -1 : line.Amount;
        }

        private static Model.Accounting.JournalLine ToModelJournalLine(this JournalLine xeroJournalLine)
        {
            return new Model.Accounting.JournalLine(xeroJournalLine.AccountCode, xeroJournalLine.AccountName, JournalType(xeroJournalLine), Math.Abs(xeroJournalLine.NetAmount));
        }

        private static JournalType JournalType(JournalLine xeroJournalLine)
        {
            return xeroJournalLine.NetAmount < 0 ? Model.Accounting.JournalType.Cr : Model.Accounting.JournalType.Dr;
        }

        public static DateTimeOffset UkCreationTime(this Journal xeroJournal)
        {
            return xeroJournal.CreatedDateUTC.ToUkDateTimeOffsetFromUtc();
        }
    }
}