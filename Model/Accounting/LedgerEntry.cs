using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Model.Accounting
{
    public class LedgerEntry
    {
        public LedgerEntry(string nominalCode, string nominalName, LedgerEntryType ledgerEntryType, decimal amount)
        {
            NominalCode = nominalCode;
            NominalName = nominalName;
            LedgerEntryType = ledgerEntryType;
            Amount = amount;
        }

        public string NominalCode { get; private set; }
        public string NominalName { get; private set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public LedgerEntryType LedgerEntryType { get; private set; }
        public decimal Amount { get; private set; }

        public override string ToString()
        {
            return String.Join(" ", LedgerEntryType, NominalCode, NominalName, Amount);
        }

        protected bool Equals(LedgerEntry other)
        {
            return string.Equals(NominalCode, other.NominalCode) && string.Equals(NominalName, other.NominalName) && LedgerEntryType == other.LedgerEntryType && Amount == other.Amount;
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
                int hashCode = (NominalCode != null ? NominalCode.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (NominalName != null ? NominalName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) LedgerEntryType;
                hashCode = (hashCode*397) ^ Amount.GetHashCode();
                return hashCode;
            }
        }
    }
}
