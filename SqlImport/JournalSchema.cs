using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SqlImport
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

        public JournalSchema(
            ISchemaColumn<int> idColumn, 
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