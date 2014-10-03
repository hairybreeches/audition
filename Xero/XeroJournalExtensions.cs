using System;
using System.Linq;
using Model;
using Model.Accounting;
using Journal = XeroApi.Model.Journal;
using JournalLine = XeroApi.Model.JournalLine;

namespace Xero
{
    static class XeroJournalExtensions
    {
        public static Model.Accounting.Journal ToModelJournal(this Journal xeroJournal)
        {            
            return new Model.Accounting.Journal(xeroJournal.JournalID, xeroJournal.CreatedDateUTC, xeroJournal.JournalDate, xeroJournal.JournalLines.Select(ToModelJournalLine));
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
            var createdDateUtc = new DateTimeOffset(xeroJournal.CreatedDateUTC, TimeSpan.Zero);
            var timezone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            var offset = timezone.GetUtcOffset(createdDateUtc);
            var localDateTime = new DateTimeOffset(createdDateUtc.UtcDateTime, TimeSpan.Zero).ToOffset(offset);
            return localDateTime;
        }
    }
}