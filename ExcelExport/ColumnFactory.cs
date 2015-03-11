using System;
using System.Collections.Generic;
using Capabilities;
using CsvExport;
using Model;

namespace ExcelExport
{
    public class ColumnFactory : IColumnFactory, IFormatterFactory
    {
        private readonly DisplayField displayField;
        private readonly string header;
        private readonly IExcelColumnFormatter formatter;

        public ColumnFactory(string header, DisplayField displayField)
            :this(header, displayField, new NoFormattingRequiredFormatter())
        {
        }

        public ColumnFactory(string header, DisplayField displayField, IExcelColumnFormatter formatter)
        {
            this.header = header;
            this.displayField = displayField;
            this.formatter = formatter;
        }

        public ICsvColumn GetColumn(ICollection<DisplayFieldName> availableFields)
        {
            if (OutputColumn(availableFields))
            {
                return new CsvColumn(header, displayField);
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
            return availableFields.Contains(displayField.Name);
        }
    }
}