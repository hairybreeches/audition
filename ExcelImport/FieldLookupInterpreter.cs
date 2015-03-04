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
    public class FieldLookupInterpreter : IDataReaderFactory, ISearcherFactoryFactory
    {
        private readonly ExcelColumnNamer namer;
        private readonly SearchActionProvider actionProvider;
        private readonly DisplayFieldProvider displayFieldProvider;

        public FieldLookupInterpreter(ExcelColumnNamer namer, SearchActionProvider actionProvider, DisplayFieldProvider displayFieldProvider)
        {
            this.namer = namer;
            this.actionProvider = actionProvider;
            this.displayFieldProvider = displayFieldProvider;
        }

        public TransactionFieldReader GetDataReader(FieldLookups lookups)
        {
            return new TransactionFieldReader(
                GetIdColumn(lookups.Id),
                GetColumn<string>(lookups.Username),
                GetDateColumn(lookups.TransactionDate),                
                GetColumn<string>(lookups.NominalCode),
                GetColumn<double>(lookups.Amount),
                GetColumn<string>(lookups.Description),
                GetColumn<string>(lookups.Type),
                GetColumn<string>(lookups.NominalName),
                GetColumn<string>(lookups.AccountCode));
        }

        public ISearcherFactory CreateSearcherFactory(FieldLookups lookups)
        {
            return new SearcherFactory(GetUnavailableSearchActions(lookups), GetDisplayableFields(lookups));
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
            return displayFieldProvider.AllFields
                .Where(field => IsDisplayable(lookups, field))
                .ToArray();
        }

        private IFieldReader<T> GetColumn<T>(int columnIndex)
        {
            return IsSet(columnIndex) ? new TypedDataReader<T>(columnIndex, namer.GetColumnName(columnIndex)) : (IFieldReader<T>) new NullDataReader<T>();
        }

        private bool IsDisplayable(FieldLookups lookups, DisplayField displayField)
        {
            return AreSet(lookups, displayField.RequiredField);
        }

        private bool AreSet(FieldLookups lookups, params IMappingField[] requiredFields)
        {
            var mappings = requiredFields.Select(field => field.GetValue(lookups));
            return mappings.All(IsSet);
        }

        private static bool IsSet(int column)
        {
            return column != -1;
        }       

        private bool IsSearchable(SearchAction searchAction, FieldLookups lookups)
        {
            return AreSet(lookups, searchAction.RequiredFields.ToArray());
        }       

        private IEnumerable<SearchAction> GetUnavailableSearchActions(FieldLookups lookups)
        {
            return actionProvider.AllSearchActions
                .Where(action => !IsSearchable(action, lookups));
        }
    }
}