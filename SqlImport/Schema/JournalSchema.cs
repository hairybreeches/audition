using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SqlImport.Schema
{
    public class JournalSchema
    {
        private readonly ISqlDataReader<string> idColumn;
        private readonly ISqlDataReader<string> usernameColumn;
        private readonly ISqlDataReader<DateTime> dateColumn;
        private readonly ISqlDataReader<DateTime> creationTimeColumn;
        private readonly ISqlDataReader<string> nominalCodeColumn;
        private readonly ISqlDataReader<double> amountColumn;
        private readonly ISqlDataReader<string> detailsColumn;
        private readonly ISqlDataReader<string> nominalCodeNameColumn;

        public JournalSchema(
            ISqlDataReader<string> idColumn, 
            ISqlDataReader<string> usernameColumn, 
            ISqlDataReader<DateTime> dateColumn, 
            ISqlDataReader<DateTime> creationTimeColumn, 
            ISqlDataReader<string> nominalCodeColumn, 
            ISqlDataReader<double> amountColumn, 
            ISqlDataReader<string> descriptionColumn, 
            ISqlDataReader<string> nominalCodeNameColumn)
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