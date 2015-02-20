using System;
using System.Data;
using Model;
using SqlImport.DataReaders;

namespace SqlImport
{
    public class TransactionFieldReader
    {
        private readonly IFieldReader<string> idColumn;
        private readonly IFieldReader<string> usernameColumn;
        private readonly IFieldReader<DateTime> dateColumn;
        private readonly IFieldReader<DateTime> creationTimeColumn;
        private readonly IFieldReader<string> nominalCodeColumn;
        private readonly IFieldReader<double> amountColumn;
        private readonly IFieldReader<string> detailsColumn;
        private readonly IFieldReader<string> nominalCodeNameColumn;

        public TransactionFieldReader(
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
            return GetField(idColumn, record, recordIndex, MappingField.Id);
        }        

        public string GetUsername(IDataRecord record, int recordIndex)
        {
            return GetField(usernameColumn, record, recordIndex, MappingField.Username);
        }
        
        public DateTime GetTransactionDate(IDataRecord record, int recordIndex)
        {
            return GetField(dateColumn, record, recordIndex, MappingField.TransactionDate);
        }
        
        public DateTime GetCreationTime(IDataRecord record, int recordIndex)
        {
            return GetField(creationTimeColumn, record, recordIndex, MappingField.EntryTime);
        }
        
        public string GetNominalCode(IDataRecord record, int recordIndex)
        {
            return GetField(nominalCodeColumn, record, recordIndex, MappingField.NominalCode);
        }
        
        public double GetAmount(IDataRecord record, int recordIndex)
        {
            return GetField(amountColumn, record, recordIndex, MappingField.Amount);
        }
        
        public string GetDescription(IDataRecord record, int recordIndex)
        {
            return GetField(detailsColumn, record, recordIndex, MappingField.Description);
        }

        public string GetNominalCodeName(IDataRecord record, int recordIndex)
        {
            return GetField(nominalCodeNameColumn, record, recordIndex, MappingField.NominalName);
        }

        private static T GetField<T>(IFieldReader<T> reader, IDataRecord record, int recordIndex, MappingField mappingField)
        {
            try
            {
                return reader.GetField(record, recordIndex);
            }
            catch (Exception e)
            {
                var message = String.Format("Could not read {0} of row {1}: {2}. It looks like the data for the {0} is incorrect. If you are importing data from Excel, please check the mapping for this column and try again.", 
                    mappingField.UserFriendlyName, recordIndex, e.Message);
                throw new SqlDataFormatUnexpectedException(message, e);
            }
            
        }
    }
}