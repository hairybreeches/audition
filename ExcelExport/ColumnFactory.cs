using System;
using System.Collections.Generic;
using CsvExport;
using Model;

namespace ExcelExport
{
    public class ColumnFactory : IColumnFactory
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

        public ICsvColumn GetColumn(ICollection<DisplayField> availableFields)
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