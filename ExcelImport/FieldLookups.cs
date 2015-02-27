using System;
using SqlImport;

namespace ExcelImport
{

    public class FieldLookups
    {
        public FieldLookups(
            int description,
            int username,
            int transactionDate,
            int accountCode,
            int accountName,
            int amount,
            int id, 
            int type)
        {
            if (transactionDate < 0)
            {
                throw new ExcelMappingException(String.Format("The {0} must be mapped", MappingField.TransactionDate));
            }
            Description = description;
            Username = username;
            TransactionDate = transactionDate;
            AccountCode = accountCode;
            AccountName = accountName;
            Amount = amount;
            Id = id;
            Type = type;
        }

        public int Description { get; private set; }
        public int Username { get; private set; }
        public int TransactionDate { get; private set; }
        public int AccountCode { get; private set; }
        public int AccountName { get; private set; }
        public int Amount { get; private set; }
        public int Id { get; private set; }
        public int Type { get; private set; }

        protected bool Equals(FieldLookups other)
        {
            return Description == other.Description && Username == other.Username && TransactionDate == other.TransactionDate && AccountCode == other.AccountCode && AccountName == other.AccountName && Amount == other.Amount && Id == other.Id && Type == other.Type;
        }

        public override string ToString()
        {
            return
                String.Format(
                    @"TransactionDate:{0}, Description:{1}, Username:{2}, AccountCode:{3}, AccountName:{4}, Amount:{5}, Id:{6} Type:{7}",
                    TransactionDate, Description, Username, AccountCode, AccountName, Amount, Id, Type);
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
                hashCode = (hashCode*397) ^ AccountCode;
                hashCode = (hashCode*397) ^ AccountName;
                hashCode = (hashCode*397) ^ Amount;
                hashCode = (hashCode*397) ^ Id;
                hashCode = (hashCode*397) ^ Type;
                return hashCode;
            }
        }
    }
}