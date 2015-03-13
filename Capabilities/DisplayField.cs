using System;
using ExcelFormatting;
using Model;

namespace Capabilities
{
    public class DisplayField
    {
        private readonly Func<SqlLedgerEntry, object> getter;
        private readonly string headerValue;
        private readonly IExcelColumnFormatter columnFormatter;

        public DisplayField(DisplayFieldName name, IMappingField requiredField, Func<SqlLedgerEntry, object> getter,
            string headerValue)
            :this(name, requiredField, getter, headerValue, new NoFormattingRequiredFormatter())
        {
            
        }
        public DisplayField(DisplayFieldName name, IMappingField requiredField, Func<SqlLedgerEntry, object> getter, string headerValue, IExcelColumnFormatter columnFormatter)
        {
            RequiredField = requiredField;
            this.getter = getter;
            this.headerValue = headerValue;
            this.columnFormatter = columnFormatter;
            Name = name;
        }

        public IMappingField RequiredField { get; private set; }
        public DisplayFieldName Name { get; private set; }

        public IExcelColumnFormatter ColumnFormatter
        {
            get { return columnFormatter; }
        }

        public object GetDisplayValue(SqlLedgerEntry entry)
        {
            return getter(entry);
        }

        public string GetHeaderValue()
        {
            return headerValue;
        }
    }
}