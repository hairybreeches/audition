using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Model.Accounting
{
    public class Journal
    {
        private readonly IList<JournalLine> lines;

        public Journal(Guid id, DateTimeOffset created, DateTime journalDate, IEnumerable<JournalLine> lines)
            :this(id.ToString(), created, journalDate, String.Empty, String.Empty, lines)
        {
            
        }

        [JsonConstructor]
        public Journal(string id, DateTimeOffset created, DateTime journalDate, string username, string description, IEnumerable<JournalLine> lines)
        {
            JournalDate = journalDate;
            Username = username;
            Description = description;
            this.lines = lines.ToList();
            Created = created;
            Id = id;            
        }        

        public string Id { get; private set; }
        public DateTimeOffset Created { get; private set; }
        public DateTime JournalDate { get; private set; }
        public string Username { get; private set; }
        public string Description { get; private set; }

        public override string ToString()
        {
            return String.Format("Id: {0}, Created: {1}, Date:{2}, Username: {3}, Description: {4} Lines: \n{5}", Id,
                Created, JournalDate, Username, Description, String.Join("\n", Lines));
        }

        public IEnumerable<JournalLine> Lines
        {
            get { return lines; }            
        }

        protected bool Equals(Journal other)
        {
            return lines.SequenceEqual(other.lines) && Id.Equals(other.Id) && Created.Equals(other.Created) && JournalDate.Equals(other.JournalDate) && Description.Equals(other.Description)
                && Username.Equals(other.Username);
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
