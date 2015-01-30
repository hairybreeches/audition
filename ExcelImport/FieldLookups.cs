using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Model;
using Searching;
using SqlImport;
using SqlImport.DataReaders;

namespace ExcelImport
{
    public class FieldLookups
    {
        public int Description { get; set; }
        public int Username { get; set; }
        public int Created { get; set; }
        public int JournalDate { get; set; }
        public int AccountCode { get; set; }
        public int AccountName { get; set; }
        public int Amount { get; set; }
        public int Id { get; set; }

        public JournalSchema ToJournalSchema()
        {
            return new JournalSchema(
                GetIdColumn(),
                GetColumn<string>(Username, "Username"),
                GetDateColumn(JournalDate, "Journal date"),
                GetDateColumn(Created, "Created"),
                GetColumn<string>(AccountCode, "Account code"),
                GetColumn<double>(Amount, "Amount"),
                GetColumn<string>(Description, "Description"),
                GetColumn<string>(AccountName, "Account name"));
        }

        private ISqlDataReader<string> GetIdColumn()
        {
            return IsSet(Id) ? (ISqlDataReader<string>) new ToStringDataColumn("Id", Id) : new RecordNumberReader();
        }

        private static ISqlDataReader<DateTime> GetDateColumn(int columnIndex, string columnName)
        {
            return IsSet(columnIndex) ? new DateTimeConverterDecorator(new SchemaColumn<string>(columnName, columnIndex)) : (ISqlDataReader<DateTime>) new NullDataReader<DateTime>();
        }

        public IEnumerable<SearchAction> GetUnavailableActions()
        {
            return Enums.GetAllValues<SearchAction>()
                .Where(x => !IsSearchable(x));
        }
        public DisplayField[] GetDisplayableFields()
        {
            return Enums.GetAllValues<DisplayField>()
                .Where(IsDisplayable).ToArray();
        }

        private static ISqlDataReader<T> GetColumn<T>(int columnIndex, string columnName)
        {
            return IsSet(columnIndex) ? new SchemaColumn<T>(columnName, columnIndex) : (ISqlDataReader<T>) new NullDataReader<T>();
        }

        private bool IsDisplayable(DisplayField displayField)
        {
            return IsSet(SetIndicator(displayField));
        }

        private static bool IsSet(int column)
        {
            return column != -1;
        }

        private bool IsSearchable(SearchAction searchAction)
        {
            return IsSet(SetIndicator(searchAction));
        }

        private int SetIndicator(SearchAction searchAction)
        {
            switch (searchAction)
            {
                case SearchAction.Accounts:
                    return AccountCode;
                case SearchAction.Date:
                    return Created;
                case SearchAction.Ending:
                    return Amount;
                case SearchAction.Hours:
                    return Created;
                case SearchAction.Users:
                    return Username;
                default:
                    throw new InvalidEnumArgumentException(String.Format("Unrecognised search action: {0}", searchAction));
            }
        }

        private int SetIndicator(DisplayField displayField)
        {
            switch (displayField)
            {
                case DisplayField.AccountCode:
                    return AccountCode;
                case DisplayField.AccountName:
                    return AccountName;
                case DisplayField.Amount:
                    return Amount;
                case DisplayField.Created:
                    return Created;
                case DisplayField.Description:
                    return Description;
                case DisplayField.JournalDate:
                    return JournalDate;
                case DisplayField.Username:
                    return Username;
                case DisplayField.JournalType:
                    return Amount;
                default:
                    throw new InvalidEnumArgumentException(String.Format("Unrecognised field name: {0}", displayField));

            }
        }
    }
}