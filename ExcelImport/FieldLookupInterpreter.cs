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
    //todo: this has too many switch statements, we are using enums when we should be using classes
    public class FieldLookupInterpreter : IDataReaderFactory, ISearcherFactoryFactory
    {
        private readonly ExcelColumnNamer namer;

        public FieldLookupInterpreter(ExcelColumnNamer namer)
        {
            this.namer = namer;
        }

        public JournalDataReader GetDataReader(FieldLookups lookups)
        {
            return new JournalDataReader(
                GetIdColumn(lookups.Id),
                GetColumn<string>(lookups.Username),
                GetDateColumn(lookups.JournalDate),
                GetDateColumn(lookups.Created),
                GetColumn<string>(lookups.AccountCode),
                GetColumn<double>(lookups.Amount),
                GetColumn<string>(lookups.Description),
                GetColumn<string>(lookups.AccountName));
        }

        public IJournalSearcherFactory CreateSearcherFactory(FieldLookups lookups)
        {
            return new JournalSearcherFactory(GetUnavailableSearchMessages(lookups), GetDisplayableFields(lookups));
        }

        private ISqlDataReader<string> GetIdColumn(int id)
        {
            return IsSet(id) ? (ISqlDataReader<string>) new ToStringDataReader(id, namer.GetColumnName(id)) : new RecordNumberReader();
        }

        private ISqlDataReader<DateTime> GetDateColumn(int columnIndex)
        {
            return IsSet(columnIndex) ? new DateTimeReader(columnIndex, namer.GetColumnName(columnIndex)) : (ISqlDataReader<DateTime>) new NullDataReader<DateTime>();
        }

        private DisplayField[] GetDisplayableFields(FieldLookups lookups)
        {
            return Enums.GetAllValues<DisplayField>()
                .Where(field => IsDisplayable(lookups, field)).ToArray();
        }

        private ISqlDataReader<T> GetColumn<T>(int columnIndex)
        {
            return IsSet(columnIndex) ? new SqlDataReader<T>(columnIndex, namer.GetColumnName(columnIndex)) : (ISqlDataReader<T>) new NullDataReader<T>();
        }

        private bool IsDisplayable(FieldLookups lookups, DisplayField displayField)
        {
            return IsSet(SetIndicator(lookups, displayField));
        }

        private static bool IsSet(int column)
        {
            return column != -1;
        }

        private int SetIndicator(FieldLookups lookups, DisplayField displayField)
        {
            switch (displayField)
            {
                case DisplayField.AccountCode:
                    return lookups.AccountCode;
                case DisplayField.AccountName:
                    return lookups.AccountName;
                case DisplayField.Amount:
                    return lookups.Amount;
                case DisplayField.Created:
                    return lookups.Created;
                case DisplayField.Description:
                    return lookups.Description;
                case DisplayField.JournalDate:
                    return lookups.JournalDate;
                case DisplayField.Username:
                    return lookups.Username;
                case DisplayField.JournalType:
                    return lookups.Amount;
                default:
                    throw new InvalidEnumArgumentException(String.Format("Unrecognised field name: {0}", displayField));

            }
        }

        private bool IsSearchable(SearchAction searchAction, FieldLookups lookups)
        {
            return IsSet(SetIndicator(searchAction, lookups));
        }

        private int SetIndicator(SearchAction searchAction, FieldLookups lookups)
        {
            switch (searchAction)
            {
                case SearchAction.Accounts:
                    return lookups.AccountCode;
                case SearchAction.Date:
                    return lookups.Created;
                case SearchAction.Ending:
                    return lookups.Amount;
                case SearchAction.Hours:
                    return lookups.Created;
                case SearchAction.Users:
                    return lookups.Username;
                default:
                    throw new InvalidEnumArgumentException(String.Format("Unrecognised search action: {0}", searchAction));
            }
        }

        private string GetErrorMessage(SearchAction action)
        {
            switch (action)
            {
                case SearchAction.Ending:
                    return
                        "In order to search for journals with round number endings, you must import journals with an amount value";
                case SearchAction.Users:
                    return
                        "In order to search for journals posted by unexpected users, you must import journals with a username value";
                case SearchAction.Date:
                    return
                        "In order to search for journals created near or after the year end, you must import journals with a creation time value";
                case SearchAction.Hours:
                    return
                        "In order to search for journals posted outside of working hours, you must import journals with a creation time value";
                case SearchAction.Accounts:
                    return
                        "In order to search for journals posted to unusual nomincal codes, you must import journals with a nominal code value";
                default:
                    throw new InvalidEnumArgumentException(String.Format("Unrecognised search action: {0}", action));
            }
        }

        private IDictionary<SearchAction, string> GetUnavailableSearchMessages(FieldLookups lookups)
        {
            return Enums.GetAllValues<SearchAction>()
                .Where(action => !IsSearchable(action, lookups))
                .Aggregate(new Dictionary<SearchAction, string>(), (dictionary, action) =>
                {
                    dictionary.Add(action, GetErrorMessage(action));
                    return dictionary;
                });
        }
    }
}