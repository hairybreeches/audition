using Model;

namespace ExcelImport
{
    public class ExcelImportMapping
    {
        public HeaderRowData SheetData { get; set; }
        public FieldLookups Lookups { get; set; }

        protected bool Equals(ExcelImportMapping other)
        {
            return Equals(SheetData, other.SheetData) && Equals(Lookups, other.Lookups);
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
                return ((SheetData != null ? SheetData.GetHashCode() : 0)*397) ^ (Lookups != null ? Lookups.GetHashCode() : 0);
            }
        }
    }
}