using System;
using System.Collections.Generic;
using System.Linq;
using SqlImport;
using SqlImport.DataReaders;

namespace Sage50.Parsing
{
    public class SageJournalSchema : JournalSchema
    {
        private static readonly ToStringDataColumn IdColumn = new ToStringDataColumn("TRAN_NUMBER", 0);
        private static readonly SchemaColumn<string> UsernameColumn = new SchemaColumn<string>("USER_NAME", 1);
        private static readonly SchemaColumn<DateTime> DateColumn = new SchemaColumn<DateTime>("DATE", 2);
        private static readonly SchemaColumn<DateTime> CreationTimeColumn = new SchemaColumn<DateTime>("RECORD_CREATE_DATE", 3);
        private static readonly SchemaColumn<string> NominalCodeColumn = new SchemaColumn<string>("NOMINAL_CODE", 4);
        private static readonly SchemaColumn<double> AmountColumn = new SchemaColumn<double>("AMOUNT", 5);
        private static readonly SchemaColumn<string> DetailsColumn = new SchemaColumn<string>("DETAILS", 6);

        public SageJournalSchema()
            :base(
                IdColumn, 
                new ColumnNameVerifierDecorator<string>(UsernameColumn), 
                new ColumnNameVerifierDecorator<DateTime>(DateColumn), 
                new ColumnNameVerifierDecorator<DateTime>(CreationTimeColumn), 
                new ColumnNameVerifierDecorator<string>(NominalCodeColumn), 
                new ColumnNameVerifierDecorator<double>(AmountColumn), 
                new ColumnNameVerifierDecorator<string>(DetailsColumn), 
                new NullDataReader<string>())
        {            
        }

        public IEnumerable<ISchemaColumn> MappedColumns
        {
            get
            {
                return new ISchemaColumn[]
                {
                    IdColumn,
                    UsernameColumn,
                    DateColumn,
                    CreationTimeColumn,
                    NominalCodeColumn,
                    AmountColumn,
                    DetailsColumn                    
                }
                .Where(x => x.Index != -1)
                .OrderBy(x => x.Index);
            }
        }

        public IEnumerable<string> ColumnNames
        {
            get { return MappedColumns.Select(x => x.FieldName); }
        }
    }
}