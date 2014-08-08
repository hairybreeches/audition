using Model;

namespace Tests
{
    static class JournalExtensions
    {
        public static XeroApi.Model.Journal ToXeroJournal(this Journal modelJournal)
        {
            return new XeroApi.Model.Journal
            {
                JournalID = modelJournal.Id,
                CreatedDateUTC = modelJournal.Created,
                JournalDate = modelJournal.JournalDate

            };
        }
    }
}
