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
            :this(id.ToString(), created, transactionDate, String.Empty, String.Empty, lines, String.Empty)
        {
            
        }

        [JsonConstructor]
        public Transaction(string id, DateTime created, DateTime transactionDate, string username, string description, IEnumerable<LedgerEntry> lines, string transactionType)
        {
            TransactionDate = transactionDate;
            Username = username;
            Description = description;
            TransactionType = transactionType;
            this.lines = lines.ToList();
            Created = created;
            Id = id;            
        }        

        public string Id { get; private set; }
        public DateTime Created { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public string Username { get; private set; }
        public string Description { get; private set; }
        public string TransactionType { get; private set; }

        public override string ToString()
        {
            return String.Format("Id: {0}, Created: {1}, Date:{2}, Username: {3}, Description: {4}, Type: {5}, Lines: \n{6}", Id,
                Created, TransactionDate, Username, Description, TransactionType, String.Join("\n", Lines));
        }

        public IEnumerable<LedgerEntry> Lines
        {
            get { return lines; }            
        }

        protected bool Equals(Transaction other)
        {
            return lines.SequenceEqual(other.lines) && string.Equals(Id, other.Id) && Created.Equals(other.Created) && TransactionDate.Equals(other.TransactionDate) && string.Equals(Username, other.Username) && string.Equals(Description, other.Description) && string.Equals(TransactionType, other.TransactionType);
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
                var hashCode = (lines != null ? lines.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Id != null ? Id.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Created.GetHashCode();
                hashCode = (hashCode*397) ^ TransactionDate.GetHashCode();
                hashCode = (hashCode*397) ^ (Username != null ? Username.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (TransactionType != null ? TransactionType.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
