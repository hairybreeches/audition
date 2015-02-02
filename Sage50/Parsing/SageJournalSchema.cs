using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using SqlImport;
using SqlImport.DataReaders;

namespace Sage50.Parsing
{
    public class SageJournalSchema
    {
        private readonly ToStringDataColumn idColumn = new ToStringDataColumn("TRAN_NUMBER", 0);
        private readonly SchemaColumn<string> usernameColumn = new SchemaColumn<string>("USER_NAME", 1);
        private readonly SchemaColumn<DateTime> dateColumn = new SchemaColumn<DateTime>("DATE", 2);
        private readonly SchemaColumn<DateTime> creationTimeColumn = new SchemaColumn<DateTime>("RECORD_CREATE_DATE", 3);
        private readonly SchemaColumn<string> nominalCodeColumn = new SchemaColumn<string>("NOMINAL_CODE", 4);
        private readonly SchemaColumn<double> amountColumn = new SchemaColumn<double>("AMOUNT", 5);
        private readonly SchemaColumn<string> detailsColumn = new SchemaColumn<string>("DETAILS", 6);        

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
                    detailsColumn                    
                }
                .OrderBy(x => x.Index);
            }
        }

        public JournalDataReader CreateJournalReader(IValueLookup<string, string> nominalCodeNameLookup)
        {
            return new JournalDataReader(
                idColumn, 
                new ColumnNameVerifierDecorator<string>(usernameColumn), 
                new ColumnNameVerifierDecorator<DateTime>(dateColumn), 
                new ColumnNameVerifierDecorator<DateTime>(creationTimeColumn), 
                new ColumnNameVerifierDecorator<string>(nominalCodeColumn), 
                new ColumnNameVerifierDecorator<double>(amountColumn), 
                new ColumnNameVerifierDecorator<string>(detailsColumn), 
                new LookupConverter<string, string>(nominalCodeColumn, nominalCodeNameLookup));
        }

        public IEnumerable<string> ColumnNames
        {
            get { return MappedColumns.Select(x => x.FieldName); }
        }
    }
}