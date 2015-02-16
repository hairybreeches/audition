using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Model.Accounting
{
    public class LedgerEntry
    {
        public LedgerEntry(string accountCode, string accountName, LedgerEntryType ledgerEntryType, decimal amount)
        {
            AccountCode = accountCode;
            AccountName = accountName;
            LedgerEntryType = ledgerEntryType;
            Amount = amount;
        }

        public string AccountCode { get; private set; }
        public string AccountName { get; private set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public LedgerEntryType LedgerEntryType { get; private set; }
        public decimal Amount { get; private set; }

        public override string ToString()
        {
            return String.Join(" ", LedgerEntryType, AccountCode, AccountName, Amount);
        }

        protected bool Equals(LedgerEntry other)
        {
            return string.Equals(AccountCode, other.AccountCode) && string.Equals(AccountName, other.AccountName) && LedgerEntryType == other.LedgerEntryType && Amount == other.Amount;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((LedgerEntry) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = (AccountCode != null ? AccountCode.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (AccountName != null ? AccountName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) LedgerEntryType;
                hashCode = (hashCode*397) ^ Amount.GetHashCode();
                return hashCode;
            }
        }
    }
}
