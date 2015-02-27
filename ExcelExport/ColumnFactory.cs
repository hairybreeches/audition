using System;
using System.Collections.Generic;
using Capabilities;
using CsvExport;
using Model;

namespace ExcelExport
{
    public class ColumnFactory : IColumnFactory, IFormatterFactory
    {
        private readonly DisplayFieldName displayField;
        private readonly string header;
        private readonly Func<SqlLedgerEntry, object> fieldSelector;
        private readonly IExcelColumnFormatter formatter;

        public ColumnFactory(string header, DisplayFieldName displayField, Func<SqlLedgerEntry, object> fieldSelector)
            :this(header, displayField, fieldSelector, new NoFormattingRequiredFormatter())
        {
        }

        public ColumnFactory(string header, DisplayFieldName displayField, Func<SqlLedgerEntry, object> fieldSelector, IExcelColumnFormatter formatter)
        {
            this.header = header;
            this.displayField = displayField;
            this.fieldSelector = fieldSelector;
            this.formatter = formatter;
        }

        public ICsvColumn GetColumn(ICollection<DisplayFieldName> availableFields)
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

        public IExcelColumnFormatter GetFormatter(ICollection<DisplayFieldName> availableFields)
        {
            return OutputColumn(availableFields) ? formatter : new ColumnDoesNotExistsFormatter();
        }

        private bool OutputColumn(ICollection<DisplayFieldName> availableFields)
        {
            return availableFields.Contains(displayField);
        }
    }
}