using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Accounting
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
            ValidateLines();
        }

        private void ValidateLines()
        {
            var sum = Lines.Select(GetLineAmount).Sum();

            if (sum != 0)
            {
                throw new InvalidJournalException(String.Format("Lines for journal {0} do not balance: {1}", Id, String.Join(",", Lines.Select(x => x.ToString()))));
            }
        }

        private decimal GetLineAmount(JournalLine line)
        {
            return line.JournalType == JournalType.Cr ? line.Amount * -1 : line.Amount;
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