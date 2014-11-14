using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Model.Accounting
{
    public class JournalLine
    {
        public JournalLine(string accountCode, string accountName, JournalType journalType, decimal amount)
        {
            AccountCode = accountCode;
            AccountName = accountName;
            JournalType = journalType;
            Amount = amount;
        }

        public string AccountCode { get; private set; }
        public string AccountName { get; private set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public JournalType JournalType { get; private set; }
        public decimal Amount { get; private set; }

        public override string ToString()
        {
            return String.Join(" ", JournalType, AccountCode, AccountName, Amount);
        }

        protected bool Equals(JournalLine other)
        {
            return string.Equals(AccountCode, other.AccountCode) && string.Equals(AccountName, other.AccountName) && JournalType == other.JournalType && Amount == other.Amount;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((JournalLine) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (AccountCode != null ? AccountCode.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (AccountName != null ? AccountName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) JournalType;
                hashCode = (hashCode*397) ^ Amount.GetHashCode();
                return hashCode;
            }
        }
    }
}
