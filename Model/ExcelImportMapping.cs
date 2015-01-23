using System;

namespace Model
{
    public class ExcelImportMapping
    {
        public string Filename { get; set; }
        public bool UseHeaderRow { get; set; }
        public FieldLookups Lookups { get; set; }

        protected bool Equals(ExcelImportMapping other)
        {
            return string.Equals(Filename, other.Filename, StringComparison.CurrentCultureIgnoreCase) && UseHeaderRow.Equals(other.UseHeaderRow) && Equals(Lookups, other.Lookups);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ExcelImportMapping) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Filename != null ? Filename.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ UseHeaderRow.GetHashCode();
                hashCode = (hashCode*397) ^ (Lookups != null ? Lookups.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}