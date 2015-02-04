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
        private readonly ExcelColumnNamer namer;

        public ExcelDataMapper(ExcelColumnNamer namer)
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

        private ISqlDataReader<string> GetIdColumn(int id)
        {
            return IsSet(id) ? (ISqlDataReader<string>) new ToStringDataReader(id, namer.GetColumnName(id)) : new RecordNumberReader();
        }

        private ISqlDataReader<DateTime> GetDateColumn(int columnIndex)
        {
            return IsSet(columnIndex) ? new DateTimeReader(columnIndex, namer.GetColumnName(columnIndex)) : (ISqlDataReader<DateTime>) new NullDataReader<DateTime>();
        }

        public DisplayField[] GetDisplayableFields(FieldLookups lookups)
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
    }
}