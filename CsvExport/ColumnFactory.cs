using System;
using System.Collections.Generic;
using Model;
using SqlImport;

namespace CsvExport
{
    public class ColumnFactory
    {
        private readonly DisplayField displayField;
        private readonly string header;
        private readonly Func<SqlLedgerEntry, object> fieldSelector;

        public ColumnFactory(string header, DisplayField displayField, Func<SqlLedgerEntry, object> fieldSelector)
        {
            this.header = header;
            this.displayField = displayField;
            this.fieldSelector = fieldSelector;
        }

        internal ICsvColumn GetColumn(ICollection<DisplayField> availableFields)
        {
            if (OutputColumn(availableFields))
            {
                return new CsvColumn(header, fieldSelector);
            }
            else
            {
                return new NullCsvColumn();
            }
        }       

        private bool OutputColumn(ICollection<DisplayField> availableFields)
        {
            return availableFields.Contains(displayField);
        }
    }
}