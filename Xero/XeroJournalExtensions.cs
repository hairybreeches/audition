using System;
using System.Linq;
using Model;
using Journal = XeroApi.Model.Journal;
using JournalLine = XeroApi.Model.JournalLine;

namespace Xero
{
    static class XeroJournalExtensions
    {
        public static Model.Journal ToModelJournal(this Journal xeroJournal)
        {            
            return new Model.Journal(xeroJournal.JournalID, xeroJournal.CreatedDateUTC, xeroJournal.JournalDate, xeroJournal.JournalLines.Select(ToModelJournalLine));
        }

        private static Model.JournalLine ToModelJournalLine(this JournalLine xeroJournalLine)
        {
            return new Model.JournalLine(xeroJournalLine.AccountCode, xeroJournalLine.AccountName, JournalType(xeroJournalLine), Math.Abs(xeroJournalLine.NetAmount));
        }

        private static JournalType JournalType(JournalLine xeroJournalLine)
        {
            return xeroJournalLine.NetAmount < 0 ? Model.JournalType.Cr : Model.JournalType.Dr;
        }
    }
}