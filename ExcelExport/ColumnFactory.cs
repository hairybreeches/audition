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
        private readonly IExcelColumnFormatter formatter;

        public ColumnFactory(DisplayField displayField)
            :this(displayField, new NoFormattingRequiredFormatter())
        {
        }

        public ColumnFactory(DisplayField displayField, IExcelColumnFormatter formatter)
        {
            this.displayField = displayField;
            this.formatter = formatter;
        }

        public ICsvColumn GetColumn(ICollection<DisplayFieldName> availableFields)
        {
            if (OutputColumn(availableFields))
            {
                return new CsvColumn(displayField);
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