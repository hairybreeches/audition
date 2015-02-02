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
    public class ExcelDataMapper
    {
        private readonly int description;
        private readonly int username;
        private readonly int created;
        private readonly int journalDate;
        private readonly int accountCode;
        private readonly int accountName;
        private readonly int amount;
        private readonly int id;

        public ExcelDataMapper(FieldLookups lookups)
        {
            description = lookups.Description;
            username = lookups.Username;
            created = lookups.Created;
            journalDate = lookups.JournalDate;
            accountCode = lookups.AccountCode;
            accountName = lookups.AccountName;
            amount = lookups.Amount;
            id = lookups.Id;
        }

        public JournalDataReader GetDataReader()
        {
            return new JournalDataReader(
                GetIdColumn(),
                GetColumn<string>(username, "Username"),
                GetDateColumn(journalDate),
                GetDateColumn(created),
                GetColumn<string>(accountCode, "Account code"),
                GetColumn<double>(amount, "Amount"),
                GetColumn<string>(description, "Description"),
                GetColumn<string>(accountName, "Account name"));
        }

        private ISqlDataReader<string> GetIdColumn()
        {
            return IsSet(id) ? (ISqlDataReader<string>) new ToStringDataReader(id) : new RecordNumberReader();
        }

        private static ISqlDataReader<DateTime> GetDateColumn(int columnIndex)
        {
            return IsSet(columnIndex) ? new DateTimeReader(columnIndex) : (ISqlDataReader<DateTime>) new NullDataReader<DateTime>();
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
            return IsSet(columnIndex) ? new SqlDataReader<T>(columnIndex) : (ISqlDataReader<T>) new NullDataReader<T>();
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
                    return accountCode;
                case SearchAction.Date:
                    return created;
                case SearchAction.Ending:
                    return amount;
                case SearchAction.Hours:
                    return created;
                case SearchAction.Users:
                    return username;
                default:
                    throw new InvalidEnumArgumentException(String.Format("Unrecognised search action: {0}", searchAction));
            }
        }

        private int SetIndicator(DisplayField displayField)
        {
            switch (displayField)
            {
                case DisplayField.AccountCode:
                    return accountCode;
                case DisplayField.AccountName:
                    return accountName;
                case DisplayField.Amount:
                    return amount;
                case DisplayField.Created:
                    return created;
                case DisplayField.Description:
                    return description;
                case DisplayField.JournalDate:
                    return journalDate;
                case DisplayField.Username:
                    return username;
                case DisplayField.JournalType:
                    return amount;
                default:
                    throw new InvalidEnumArgumentException(String.Format("Unrecognised field name: {0}", displayField));

            }
        }
    }
}