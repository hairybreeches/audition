using System;
using System.Data;
using SqlImport.DataReaders;

namespace SqlImport
{
    public class JournalDataReader
    {
        private readonly ISqlDataReader<string> idColumn;
        private readonly ISqlDataReader<string> usernameColumn;
        private readonly ISqlDataReader<DateTime> dateColumn;
        private readonly ISqlDataReader<DateTime> creationTimeColumn;
        private readonly ISqlDataReader<string> nominalCodeColumn;
        private readonly ISqlDataReader<double> amountColumn;
        private readonly ISqlDataReader<string> detailsColumn;
        private readonly ISqlDataReader<string> nominalCodeNameColumn;

        public JournalDataReader(
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
            return GetField(idColumn, record, recordIndex, "ID");
        }        

        public string GetUsername(IDataRecord record, int recordIndex)
        {
            return GetField(usernameColumn, record, recordIndex, "username");
        }
        
        public DateTime GetJournalDate(IDataRecord record, int recordIndex)
        {
            return GetField(dateColumn, record, recordIndex, "journal date");
        }
        
        public DateTime GetCreationTime(IDataRecord record, int recordIndex)
        {
            return GetField(creationTimeColumn, record, recordIndex, "journal creation time");
        }
        
        public string GetNominalCode(IDataRecord record, int recordIndex)
        {
            return GetField(nominalCodeColumn, record, recordIndex, "nominal code");
        }
        
        public double GetAmount(IDataRecord record, int recordIndex)
        {
            return GetField(amountColumn, record, recordIndex, "amount");
        }
        
        public string GetDescription(IDataRecord record, int recordIndex)
        {
            return GetField(detailsColumn, record, recordIndex, "description");
        }

        public string GetNominalCodeName(IDataRecord record, int recordIndex)
        {
            return GetField(nominalCodeNameColumn, record, recordIndex, "nominal code name");
        }

        private static T GetField<T>(ISqlDataReader<T> reader, IDataRecord record, int recordIndex, string userFriendlyFieldName)
        {
            try
            {
                return reader.GetField(record, recordIndex);
            }
            catch (Exception e)
            {
                var message = String.Format("Could not read {0} of row {1}: {2}", 
                    userFriendlyFieldName, recordIndex, e.Message);
                throw new SqlDataFormatUnexpectedException(message, e);
            }
            
        }
    }
}