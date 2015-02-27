using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Capabilities;
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
        private readonly SearchActionProvider actionProvider;

        public FieldLookupInterpreter(ExcelColumnNamer namer, SearchActionProvider actionProvider)
        {
            this.namer = namer;
            this.actionProvider = actionProvider;
        }

        public TransactionFieldReader GetDataReader(FieldLookups lookups)
        {
            return new TransactionFieldReader(
                GetIdColumn(lookups.Id),
                GetColumn<string>(lookups.Username),
                GetDateColumn(lookups.TransactionDate),                
                GetColumn<string>(lookups.AccountCode),
                GetColumn<double>(lookups.Amount),
                GetColumn<string>(lookups.Description),
                GetColumn<string>(lookups.Type),
                GetColumn<string>(lookups.AccountName));
        }

        public ISearcherFactory CreateSearcherFactory(FieldLookups lookups)
        {
            return new SearcherFactory(GetUnavailableSearchMessages(lookups), GetDisplayableFields(lookups));
        }

        private IFieldReader<string> GetIdColumn(int id)
        {
            return IsSet(id) ? (IFieldReader<string>) new ToStringDataReader(id, namer.GetColumnName(id)) : new RecordNumberReader();
        }

        private IFieldReader<DateTime> GetDateColumn(int columnIndex)
        {
            return IsSet(columnIndex) ? new DateTimeReader(columnIndex, namer.GetColumnName(columnIndex)) : (IFieldReader<DateTime>) new NullDataReader<DateTime>();
        }

        private DisplayField[] GetDisplayableFields(FieldLookups lookups)
        {
            return Enums.GetAllValues<DisplayField>()
                .Where(field => IsDisplayable(lookups, field)).ToArray();
        }

        private IFieldReader<T> GetColumn<T>(int columnIndex)
        {
            return IsSet(columnIndex) ? new TypedDataReader<T>(columnIndex, namer.GetColumnName(columnIndex)) : (IFieldReader<T>) new NullDataReader<T>();
        }

        private bool IsDisplayable(FieldLookups lookups, DisplayField displayField)
        {
            return IsSet(RequiredField(displayField), lookups);
        }

        private bool IsSet(IMappingField requiredField, FieldLookups lookups)
        {
            var mapping = requiredField.GetValue(lookups);
            return IsSet(mapping);
        }

        private static bool IsSet(int column)
        {
            return column != -1;
        }

        private IMappingField RequiredField(DisplayField displayField)
        {
            switch (displayField)
            {
                case DisplayField.AccountCode:
                    return MappingFields.NominalCode;
                case DisplayField.AccountName:
                    return MappingFields.NominalName;
                case DisplayField.Amount:
                    return MappingFields.Amount;
                case DisplayField.Description:
                    return MappingFields.Description;
                case DisplayField.TransactionDate:
                    return MappingFields.TransactionDate;
                case DisplayField.Username:
                    return MappingFields.Username;
                case DisplayField.LedgerEntryType:
                    return MappingFields.Amount;
                case DisplayField.Id:
                    return MappingFields.Id;   
                case DisplayField.Type:
                    return MappingFields.Type;
                default:
                    throw new InvalidEnumArgumentException(String.Format("Unrecognised field name: {0}", displayField));

            }
        }

        private bool IsSearchable(SearchAction searchAction, FieldLookups lookups)
        {
            return IsSet(searchAction.RequiredField, lookups);
        }       

        private IDictionary<SearchActionName, string> GetUnavailableSearchMessages(FieldLookups lookups)
        {
            return actionProvider.AllSearchActions
                .Where(action => !IsSearchable(action, lookups))
                .Aggregate(new Dictionary<SearchActionName, string>(), (dictionary, action) =>
                {
                    dictionary.Add(action.Name, action.ErrorMessage);
                    return dictionary;
                });
        }
    }
}