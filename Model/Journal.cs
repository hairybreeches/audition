using System;

namespace Model
{
    public class Journal
    {
        public Guid Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime JournalDate { get; set; }
    }
}