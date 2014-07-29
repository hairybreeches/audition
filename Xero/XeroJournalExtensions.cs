using XeroApi.Model;

namespace Xero
{
    static class XeroJournalExtensions
    {
        public static Model.Journal ToModelJournal(this Journal xeroJournal)
        {
            return new Model.Journal(xeroJournal.JournalID, xeroJournal.CreatedDateUTC, xeroJournal.JournalDate);        
        }
    }
}