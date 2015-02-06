using System;
using System.Collections.Generic;
using System.Linq;

namespace ExcelImport
{
    public class SheetMetadata
    {
        public SheetMetadata(string name, IEnumerable<string> columnNames, IEnumerable<string> columnHeaders)
        {
            Name = name;
            ColumnNames = columnNames;
            ColumnHeaders = columnHeaders;
        }

        public string Name { get; private set; }
        public IEnumerable<string> ColumnNames { get; private set; }
        public IEnumerable<string> ColumnHeaders { get; private set; }

        protected bool Equals(SheetMetadata other)
        {
            return string.Equals(Name, other.Name) && ColumnNames.SequenceEqual(other.ColumnNames) && ColumnHeaders.SequenceEqual(other.ColumnHeaders);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SheetMetadata) obj);
        }

        public override string ToString()
        {
            return Name 
                + "\r\nColumn headers: " + String.Join(", ", ColumnHeaders)
                + "\r\nColumns: " + String.Join(", ", ColumnNames);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ColumnNames != null ? ColumnNames.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (ColumnHeaders != null ? ColumnHeaders.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}