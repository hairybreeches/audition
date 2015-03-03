using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Model.Accounting
{
    public class Transaction
    {
        private readonly IList<LedgerEntry> lines;

        [JsonConstructor]
        public Transaction(string id, DateTime transactionDate, string username, string description, string transactionType, IEnumerable<LedgerEntry> lines)
        {
            TransactionDate = transactionDate;
            Username = username;
            Description = description;
            TransactionType = transactionType;
            this.lines = lines.ToList();
            Id = id;            
        }        

        public string Id { get; private set; }
        public DateTime TransactionDate { get; private set; }
        public string Username { get; private set; }
        public string Description { get; private set; }
        public string TransactionType { get; private set; }

        public override string ToString()
        {
            return String.Format("Id: {0}, Date:{1}, Username: {2}, Description: {3}, Type: {4}, Lines: \n{5}", Id,
                TransactionDate, Username, Description, TransactionType, String.Join("\n", Lines));
        }

        public IEnumerable<LedgerEntry> Lines
        {
            get { return lines; }            
        }

        protected bool Equals(Transaction other)
        {
            return lines.SequenceEqual(other.lines) && string.Equals(Id, other.Id) && TransactionDate.Equals(other.TransactionDate) && string.Equals(Username, other.Username) && string.Equals(Description, other.Description) && string.Equals(TransactionType, other.TransactionType);
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
                hashCode = (hashCode*397) ^ TransactionDate.GetHashCode();
                hashCode = (hashCode*397) ^ (Username != null ? Username.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (TransactionType != null ? TransactionType.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
