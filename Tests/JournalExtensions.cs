using Model;

namespace Tests
{
    static class JournalExtensions
    {
        public static XeroApi.Model.Journal ToXeroJournal(this Journal xeroJournal)
        {
            return new XeroApi.Model.Journal
            {
                JournalID = xeroJournal.Id,
                CreatedDateUTC = xeroJournal.Created,
                JournalDate = xeroJournal.JournalDate
            };
        }
    }
}
