using System;
using System.Data;
using Capabilities;
using Model;
using SqlImport.DataReaders;

namespace SqlImport
{
    public class TransactionFieldReader
    {
        private readonly IFieldReader<string> idColumn;
        private readonly IFieldReader<string> usernameColumn;
        private readonly IFieldReader<DateTime> dateColumn;
        private readonly IFieldReader<string> nominalCodeColumn;
        private readonly IFieldReader<double> amountColumn;
        private readonly IFieldReader<string> detailsColumn;
        private readonly IFieldReader<string> typeColumn;
        private readonly IFieldReader<string> nominalCodeNameColumn;

        public TransactionFieldReader(
            IFieldReader<string> idColumn, 
            IFieldReader<string> usernameColumn, 
            IFieldReader<DateTime> dateColumn,              
            IFieldReader<string> nominalCodeColumn, 
            IFieldReader<double> amountColumn, 
            IFieldReader<string> descriptionColumn, 
            IFieldReader<string> typeColumn, 
            IFieldReader<string> nominalCodeNameColumn)
        {
            this.idColumn = idColumn;
            this.usernameColumn = usernameColumn;
            this.dateColumn = dateColumn;
            this.nominalCodeColumn = nominalCodeColumn;
            this.amountColumn = amountColumn;
            detailsColumn = descriptionColumn;
            this.typeColumn = typeColumn;
            this.nominalCodeNameColumn = nominalCodeNameColumn;
        }                

        

        public string GetId(IDataRecord record, int recordIndex)
        {
            return GetField(idColumn, record, recordIndex, MappingFields.Id);
        }        

        public string GetUsername(IDataRecord record, int recordIndex)
        {
            return GetField(usernameColumn, record, recordIndex, MappingFields.Username);
        }
        
        public string GetType(IDataRecord record, int recordIndex)
        {
            return GetField(typeColumn, record, recordIndex, MappingFields.Type);
        }
        
        public DateTime GetTransactionDate(IDataRecord record, int recordIndex)
        {
            return GetField(dateColumn, record, recordIndex, MappingFields.TransactionDate);
        }
        
        public string GetNominalCode(IDataRecord record, int recordIndex)
        {
            return GetField(nominalCodeColumn, record, recordIndex, MappingFields.NominalCode);
        }
        
        public double GetAmount(IDataRecord record, int recordIndex)
        {
            return GetField(amountColumn, record, recordIndex, MappingFields.Amount);
        }
        
        public string GetDescription(IDataRecord record, int recordIndex)
        {
            return GetField(detailsColumn, record, recordIndex, MappingFields.Description);
        }

        public string GetNominalCodeName(IDataRecord record, int recordIndex)
        {
            return GetField(nominalCodeNameColumn, record, recordIndex, MappingFields.NominalName);
        }

        private static T GetField<T>(IFieldReader<T> reader, IDataRecord record, int recordIndex, IMappingField mappingField)
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