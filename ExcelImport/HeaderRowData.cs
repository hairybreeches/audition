using System;

namespace ExcelImport
{
    public class HeaderRowData
    {
        public string Filename { get; set; }
        public int Sheet { get; set; }
        public bool UseHeaderRow { get; set; }

        protected bool Equals(HeaderRowData other)
        {
            return string.Equals(Filename, other.Filename, StringComparison.InvariantCultureIgnoreCase) && Sheet == other.Sheet && UseHeaderRow.Equals(other.UseHeaderRow);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HeaderRowData) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Filename != null ? Filename.ToLowerInvariant().GetHashCode() : 0);
                hashCode = (hashCode*397) ^ Sheet;
                hashCode = (hashCode*397) ^ UseHeaderRow.GetHashCode();
                return hashCode;
            }
        }
    }
}