using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using SqlImport;

namespace Sage50.Parsing.Schema
{
    public class JournalSchema
    {
        private readonly ISchemaColumn<int> idColumn;
        private readonly ISchemaColumn<string> usernameColumn;
        private readonly ISchemaColumn<DateTime> dateColumn;
        private readonly ISchemaColumn<DateTime> creationTimeColumn;
        private readonly ISchemaColumn<string> nominalCodeColumn;
        private readonly ISchemaColumn<double> amountColumn;
        private readonly ISchemaColumn<string> detailsColumn;
        private readonly ISchemaColumn<string> nominalCodeNameColumn;

        public JournalSchema()
        {
            idColumn = new SchemaColumn<Int32>("TRAN_NUMBER", 0);
            usernameColumn = new SchemaColumn<string>("USER_NAME", 1);
            dateColumn = new SchemaColumn<DateTime>("DATE", 2);
            creationTimeColumn = new SchemaColumn<DateTime>("RECORD_CREATE_DATE", 3);
            nominalCodeColumn = new SchemaColumn<string>("NOMINAL_CODE", 4);
            amountColumn = new SchemaColumn<double>("AMOUNT", 5);
            detailsColumn = new SchemaColumn<string>("DETAILS", 6);
            nominalCodeNameColumn = new UnmappedColumn<string>();
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
                    creationTimeColumn,
                    nominalCodeColumn,
                    amountColumn,
                    detailsColumn,
                    nominalCodeNameColumn
                }
                .Where(x => x.Index != -1)
                .OrderBy(x => x.Index);
            }
        }

        public IEnumerable<string> ColumnNames
        {
            get { return MappedColumns.Select(x=>x.FieldName); }
        }

        public int GetId(IDataRecord record)
        {
            return idColumn.GetField(record);
        }
        
        public string GetUsername(IDataRecord record)
        {
            return usernameColumn.GetField(record);
        }
        
        public DateTime GetJournalDate(IDataRecord record)
        {
            return dateColumn.GetField(record);
        }
        
        public DateTime GetCreationTime(IDataRecord record)
        {
            return creationTimeColumn.GetField(record);
        }
        
        public string GetNominalCode(IDataRecord record)
        {
            return nominalCodeColumn.GetField(record);
        }
        
        public double GetAmount(IDataRecord record)
        {
            return amountColumn.GetField(record);
        }
        
        public string GetDescription(IDataRecord record)
        {
            return detailsColumn.GetField(record);
        }

        public string GetNominalCodeName(IDataRecord record)
        {
            return nominalCodeNameColumn.GetField(record);
        }
    }
}