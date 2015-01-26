namespace ExcelImport
{
    public class FieldLookups
    {
        public int Description { get; set; }
        public int Username { get; set; }
        public int Created { get; set; }
        public int JournalDate { get; set; }
        public int AccountCode { get; set; }
        public int AccountName { get; set; }
        public int Amount { get; set; }

        protected bool Equals(FieldLookups other)
        {
            return string.Equals(Description, other.Description)
                   && string.Equals(Username, other.Username)
                   && string.Equals(Created, other.Created)
                   && string.Equals(JournalDate, other.JournalDate)
                   && string.Equals(AccountCode, other.AccountCode)
                   && string.Equals(AccountName, other.AccountName)
                   && string.Equals(Amount, other.Amount);
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
                return hashCode;
            }
        }
    }
}