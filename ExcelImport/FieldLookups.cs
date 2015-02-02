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
        public FieldLookups(
            int description, 
            int username, 
            int created, 
            int journalDate, 
            int accountCode, 
            int accountName, 
            int amount, 
            int id)
        {
            Description = description;
            Username = username;
            Created = created;
            JournalDate = journalDate;
            AccountCode = accountCode;
            AccountName = accountName;
            Amount = amount;
            Id = id;
        }

        public int Description { get; private set; }
        public int Username { get; private set; }
        public int Created { get; private set; }
        public int JournalDate { get; private set; }
        public int AccountCode { get; private set; }
        public int AccountName { get; private set; }
        public int Amount { get; private set; }
        public int Id { get; private set; }

        public JournalDataReader ToJournalSchema()
        {
            return new JournalDataReader(
                GetIdColumn(),
                GetColumn<string>(Username, "Username"),
                GetDateColumn(JournalDate),
                GetDateColumn(Created),
                GetColumn<string>(AccountCode, "Account code"),
                GetColumn<double>(Amount, "Amount"),
                GetColumn<string>(Description, "Description"),
                GetColumn<string>(AccountName, "Account name"));
        }

        private ISqlDataReader<string> GetIdColumn()
        {
            return IsSet(Id) ? (ISqlDataReader<string>) new ToStringDataReader(Id) : new RecordNumberReader();
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
            return IsSet(columnIndex) ? new SqlDataReader<T>(columnName, columnIndex) : (ISqlDataReader<T>) new NullDataReader<T>();
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