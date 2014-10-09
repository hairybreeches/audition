using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Accounting
{
    public class Journal
    {
        private readonly IList<JournalLine> lines;

        public Journal(Guid id, DateTimeOffset created, DateTime journalDate, IEnumerable<JournalLine> lines)
            :this(id.ToString(), created, journalDate, String.Empty, String.Empty, lines)
        {
            
        }
        public Journal(string id, DateTimeOffset created, DateTime journalDate, string username, string description, IEnumerable<JournalLine> lines)
        {
            JournalDate = journalDate;
            Username = username;
            Description = description;
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

        public string Id { get; private set; }
        public DateTimeOffset Created { get; private set; }
        public DateTime JournalDate { get; private set; }
        public string Username { get; private set; }
        public string Description { get; private set; }

        public IEnumerable<JournalLine> Lines
        {
            get { return lines; }            
        }

        protected bool Equals(Journal other)
        {
            return lines.SequenceEqual(other.lines) && Id.Equals(other.Id) && Created.Equals(other.Created) && JournalDate.Equals(other.JournalDate);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Journal) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (lines != null ? lines.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Id.GetHashCode();
                hashCode = (hashCode*397) ^ Created.GetHashCode();
                hashCode = (hashCode*397) ^ JournalDate.GetHashCode();
                return hashCode;
            }
        }
    }
}