using System;

namespace Model
{
    public class Journal
    {
        public Journal(Guid id, DateTime created, DateTime journalDate)
        {
            JournalDate = journalDate;
            Created = created;
            Id = id;
        }

        public Guid Id { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime JournalDate { get; private set; }
    }
}