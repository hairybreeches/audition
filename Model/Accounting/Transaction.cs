using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Model.Accounting
{
    public class Transaction
    {
        private readonly IList<LedgerEntry> lines;

        public Transaction(Guid id, DateTime created, DateTime transactionDate, IEnumerable<LedgerEntry> lines)
            :this(id.ToString(), created, transactionDate, String.Empty, String.Empty, lines)
        {
            
        }

        [JsonConstructor]
        public Transaction(string id, DateTime created, DateTime transactionDate, string username, string description, IEnumerable<LedgerEntry> lines)
        {
            TransactionDate = transactionDate;
            Username = username;
            Description = description;
            this.lines = lines.ToList();
            Created = created;
            Id = id;            
        }        

        public string Id { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public string Username { get; private set; }
        public string Description { get; private set; }

        public override string ToString()
        {
            return String.Format("Id: {0}, Created: {1}, Date:{2}, Username: {3}, Description: {4} Lines: \n{5}", Id,
                Created, TransactionDate, Username, Description, String.Join("\n", Lines));
        }

        public IEnumerable<LedgerEntry> Lines
        {
            get { return lines; }            
        }

        protected bool Equals(Transaction other)
        {
            return lines.SequenceEqual(other.lines) && Id.Equals(other.Id) && Created.Equals(other.Created) && TransactionDate.Equals(other.TransactionDate) && Description.Equals(other.Description)
                && Username.Equals(other.Username);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Transaction) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (lines != null ? lines.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Id.GetHashCode();
                hashCode = (hashCode*397) ^ Created.GetHashCode();
                hashCode = (hashCode*397) ^ TransactionDate.GetHashCode();
                return hashCode;
            }
        }
    }
}
