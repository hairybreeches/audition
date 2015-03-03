using System;
using Capabilities;
using SqlImport;

namespace ExcelImport
{

    public class FieldLookups : IMapping<int>
    {
        public FieldLookups(
            int description,
            int username,
            int transactionDate,
            int nominalCode,
            int nominalName,
            int amount,
            int id, 
            int type)
        {            
            Description = description;
            Username = username;
            TransactionDate = transactionDate;
            NominalCode = nominalCode;
            NominalName = nominalName;
            Amount = amount;
            Id = id;
            Type = type;
        }

        public int Description { get; private set; }
        public int Username { get; private set; }
        public int TransactionDate { get; private set; }
        public int NominalCode { get; private set; }
        public int NominalName { get; private set; }
        public int Amount { get; private set; }
        public int Id { get; private set; }
        public int Type { get; private set; }

        protected bool Equals(FieldLookups other)
        {
            return Description == other.Description && Username == other.Username && TransactionDate == other.TransactionDate && NominalCode == other.NominalCode && NominalName == other.NominalName && Amount == other.Amount && Id == other.Id && Type == other.Type;
        }

        public override string ToString()
        {
            return
                String.Format(
                    @"TransactionDate:{0}, Description:{1}, Username:{2}, NominalCode:{3}, NominalName:{4}, Amount:{5}, Id:{6} Type:{7}",
                    TransactionDate, Description, Username, NominalCode, NominalName, Amount, Id, Type);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FieldLookups) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Description;
                hashCode = (hashCode*397) ^ Username;
                hashCode = (hashCode*397) ^ TransactionDate;
                hashCode = (hashCode*397) ^ NominalCode;
                hashCode = (hashCode*397) ^ NominalName;
                hashCode = (hashCode*397) ^ Amount;
                hashCode = (hashCode*397) ^ Id;
                hashCode = (hashCode*397) ^ Type;
                return hashCode;
            }
        }
    }
}