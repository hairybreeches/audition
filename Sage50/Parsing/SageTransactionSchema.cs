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
        private readonly SchemaColumn<string> idColumn = new SchemaColumn<string>("TRAN_NUMBER", 0, (name, index) => new ToStringDataReader(index, name));
        private readonly SchemaColumn<string> usernameColumn = new SchemaColumn<string>("USER_NAME", 1);
        private readonly SchemaColumn<DateTime> dateColumn = new SchemaColumn<DateTime>("DATE", 2);
        private readonly SchemaColumn<DateTime> creationTimeColumn = new SchemaColumn<DateTime>("RECORD_CREATE_DATE", 3);
        private readonly SchemaColumn<string> nominalCodeColumn = new SchemaColumn<string>("NOMINAL_CODE", 4);
        private readonly SchemaColumn<double> amountColumn = new SchemaColumn<double>("AMOUNT", 5);
        private readonly SchemaColumn<string> detailsColumn = new SchemaColumn<string>("DETAILS", 6);        
        private readonly SchemaColumn<string> typeColumn = new SchemaColumn<string>("TYPE", 7);        

        public IEnumerable<ISchemaColumn> MappedColumns
        {
            get
            {
                return new ISchemaColumn[]
                {
                    idColumn,
                    usernameColumn,
                    dateColumn,
                    creationTimeColumn,
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
                creationTimeColumn.DataReader, 
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