using System;
using System.Collections.Generic;
using Model;

namespace CsvExport
{
    public class ColumnFactory<TRecord>
    {
        private readonly DisplayField displayField;
        private readonly string header;
        private readonly Func<TRecord, object> fieldSelector;

        public ColumnFactory(string header, DisplayField displayField, Func<TRecord, object> fieldSelector)
        {
            this.header = header;
            this.displayField = displayField;
            this.fieldSelector = fieldSelector;
        }

        internal ICsvColumn<TRecord> GetColumn(ICollection<DisplayField> availableFields)
        {
            if (availableFields.Contains(displayField))
            {
                return new CsvColumn<TRecord>(header, fieldSelector);
            }
            else
            {
                return new NullCsvColumn<TRecord>();
            }
        }


    }
}