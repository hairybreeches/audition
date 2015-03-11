using System;
using Model;

namespace Capabilities
{
    public class DisplayField
    {
        private readonly Func<SqlLedgerEntry, object> getter;
        private readonly string headerValue;

        public DisplayField(DisplayFieldName name, IMappingField requiredField, Func<SqlLedgerEntry, object> getter, string headerValue)
        {
            RequiredField = requiredField;
            this.getter = getter;
            this.headerValue = headerValue;
            Name = name;
        }

        public IMappingField RequiredField { get; private set; }
        public DisplayFieldName Name { get; private set; }

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