using System;
using Model;

namespace Capabilities
{
    public class DisplayField
    {
        private readonly Func<SqlLedgerEntry, object> getter;

        public DisplayField(DisplayFieldName name, IMappingField requiredField, Func<SqlLedgerEntry, object> getter)
        {
            RequiredField = requiredField;
            this.getter = getter;
            Name = name;
        }

        public IMappingField RequiredField { get; private set; }
        public DisplayFieldName Name { get; private set; }

        public object GetDisplayValue(SqlLedgerEntry entry)
        {
            return getter(entry);
        }
    }
}