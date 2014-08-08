using System;
using System.Collections.Generic;
using System.Linq;

namespace Model
{
    public class Journal
    {
        private readonly IList<JournalLine> lines;

        public Journal(Guid id, DateTime created, DateTime journalDate, IEnumerable<JournalLine> lines)
        {
            JournalDate = journalDate;
            this.lines = lines.ToList();
            Created = created;
            Id = id;
        }

        public Guid Id { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime JournalDate { get; private set; }

        public IEnumerable<JournalLine> Lines
        {
            get { return lines; }            
        }
    }
}