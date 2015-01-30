using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SqlImport.Schema
{
    public class JournalSchema
    {
        private readonly ISchemaColumn<string> idColumn;
        private readonly ISchemaColumn<string> usernameColumn;
        private readonly ISchemaColumn<DateTime> dateColumn;
        private readonly ISchemaColumn<DateTime> creationTimeColumn;
        private readonly ISchemaColumn<string> nominalCodeColumn;
        private readonly ISchemaColumn<double> amountColumn;
        private readonly ISchemaColumn<string> detailsColumn;
        private readonly ISchemaColumn<string> nominalCodeNameColumn;

        public JournalSchema(
            ISchemaColumn<string> idColumn, 
            ISchemaColumn<string> usernameColumn, 
            ISchemaColumn<DateTime> dateColumn, 
            ISchemaColumn<DateTime> creationTimeColumn, 
            ISchemaColumn<string> nominalCodeColumn, 
            ISchemaColumn<double> amountColumn, 
            ISchemaColumn<string> descriptionColumn, 
            ISchemaColumn<string> nominalCodeNameColumn)
        {
            this.idColumn = idColumn;
            this.usernameColumn = usernameColumn;
            this.dateColumn = dateColumn;
            this.creationTimeColumn = creationTimeColumn;
            this.nominalCodeColumn = nominalCodeColumn;
            this.amountColumn = amountColumn;
            detailsColumn = descriptionColumn;
            this.nominalCodeNameColumn = nominalCodeNameColumn;
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

        public string GetId(IDataRecord record, int recordIndex)
        {
            return idColumn.GetField(record, recordIndex);
        }
        
        public string GetUsername(IDataRecord record, int recordIndex)
        {
            return usernameColumn.GetField(record, recordIndex);
        }
        
        public DateTime GetJournalDate(IDataRecord record, int recordIndex)
        {
            return dateColumn.GetField(record, recordIndex);
        }
        
        public DateTime GetCreationTime(IDataRecord record, int recordIndex)
        {
            return creationTimeColumn.GetField(record, recordIndex);
        }
        
        public string GetNominalCode(IDataRecord record, int recordIndex)
        {
            return nominalCodeColumn.GetField(record, recordIndex);
        }
        
        public double GetAmount(IDataRecord record, int recordIndex)
        {
            return amountColumn.GetField(record, recordIndex);
        }
        
        public string GetDescription(IDataRecord record, int recordIndex)
        {
            return detailsColumn.GetField(record, recordIndex);
        }

        public string GetNominalCodeName(IDataRecord record, int recordIndex)
        {
            return nominalCodeNameColumn.GetField(record, recordIndex);
        }
    }
}