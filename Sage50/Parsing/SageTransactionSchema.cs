using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using SqlImport;
using SqlImport.DataReaders;

namespace Sage50.Parsing
{
    public class SageTransactionSchema
    {
        private readonly SchemaColumn<string> idColumn = new SchemaColumn<string>("TRAN_NUMBER", 0, CreateToStringDataReader);
        private readonly SchemaColumn<string> usernameColumn = new SchemaColumn<string>("USER_NAME", 1, CreateOptionalStringColumnReader);
        private readonly SchemaColumn<DateTime> dateColumn = new SchemaColumn<DateTime>("DATE", 2);
        private readonly SchemaColumn<string> nominalCodeColumn = new SchemaColumn<string>("NOMINAL_CODE", 3);
        private readonly SchemaColumn<double> amountColumn = new SchemaColumn<double>("AMOUNT", 4);
        private readonly SchemaColumn<string> detailsColumn = new SchemaColumn<string>("DETAILS", 5, CreateOptionalStringColumnReader);
        private readonly SchemaColumn<string> typeColumn = new SchemaColumn<string>("TYPE", 6, CreateNominalLookupReader);
        private const string DefaultString = "<none>";

        private static IFieldReader<string> CreateToStringDataReader(string name, int index)
        {
            return new ToStringDataReader(index, name);
        }
        private static IFieldReader<string> CreateNominalLookupReader(string name, int index)
        {
            return new LookupConverter<string,string>(new ToStringDataReader(index, name), new Sage50TransactionTypeLookup());
        }

        private static IFieldReader<string> CreateOptionalStringColumnReader(string columnName, int columnIndex)
        {
            return new TypedDataReader<string>(new DefaultValueSupplier(new FieldReader(columnIndex), DefaultString), columnName);
        }

        public IEnumerable<ISchemaColumn> MappedColumns
        {
            get
            {
                return new ISchemaColumn[]
                {
                    idColumn,
                    usernameColumn,
                    dateColumn,
                    nominalCodeColumn,
                    amountColumn,
                    detailsColumn,
                    typeColumn
                }
                .OrderBy(x => x.Index);
            }
        }

        public TransactionFieldReader CreateJournalReader(IValueLookup<string, string> nominalCodeNameLookup)
        {
            return new TransactionFieldReader(
                idColumn.DataReader, 
                usernameColumn.DataReader, 
                dateColumn.DataReader,                 
                nominalCodeColumn.DataReader, 
                amountColumn.DataReader, 
                detailsColumn.DataReader, 
                typeColumn.DataReader,
                new LookupConverter<string, string>(nominalCodeColumn.DataReader, nominalCodeNameLookup));
        }

        public IEnumerable<string> ColumnNames
        {
            get { return MappedColumns.Select(x => x.FieldName); }
        }
    }
}