using System;
using System.Data;
using SqlImport.DataReaders;

namespace SqlImport
{
    public class JournalDataReader
    {
        private readonly IFieldReader<string> idColumn;
        private readonly IFieldReader<string> usernameColumn;
        private readonly IFieldReader<DateTime> dateColumn;
        private readonly IFieldReader<DateTime> creationTimeColumn;
        private readonly IFieldReader<string> nominalCodeColumn;
        private readonly IFieldReader<double> amountColumn;
        private readonly IFieldReader<string> detailsColumn;
        private readonly IFieldReader<string> nominalCodeNameColumn;

        public JournalDataReader(
            IFieldReader<string> idColumn, 
            IFieldReader<string> usernameColumn, 
            IFieldReader<DateTime> dateColumn, 
            IFieldReader<DateTime> creationTimeColumn, 
            IFieldReader<string> nominalCodeColumn, 
            IFieldReader<double> amountColumn, 
            IFieldReader<string> descriptionColumn, 
            IFieldReader<string> nominalCodeNameColumn)
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
        
        public DateTime GetTransactionDate(IDataRecord record, int recordIndex)
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

        private static T GetField<T>(IFieldReader<T> reader, IDataRecord record, int recordIndex, string userFriendlyFieldName)
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