namespace Model
{
    public class FieldLookups
    {
        public string Description { get; set; }
        public string Username { get; set; }
        public string Created { get; set; }
        public string JournalDate { get; set; }
        public string AccountCode { get; set; }
        public string AccountName { get; set; }
        public string Amount { get; set; }
        public string JournalType { get; set; }

        protected bool Equals(FieldLookups other)
        {
            return string.Equals(Description, other.Description) && string.Equals(Username, other.Username) && string.Equals(Created, other.Created) && string.Equals(JournalDate, other.JournalDate) && string.Equals(AccountCode, other.AccountCode) && string.Equals(AccountName, other.AccountName) && string.Equals(Amount, other.Amount) && string.Equals(JournalType, other.JournalType);
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
                var hashCode = (Description != null ? Description.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Username != null ? Username.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Created != null ? Created.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (JournalDate != null ? JournalDate.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (AccountCode != null ? AccountCode.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (AccountName != null ? AccountName.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (Amount != null ? Amount.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (JournalType != null ? JournalType.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}