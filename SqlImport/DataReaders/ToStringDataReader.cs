using System;
using System.Data;

namespace SqlImport.DataReaders
{
    public class ToStringDataReader : IFieldReader<string>
    {
        private readonly string userFriendlyColumnName;
        private readonly IFieldReader<object> inner;

        public ToStringDataReader(int index, string userFriendlyColumnName)
        {
            this.inner = new FieldReader(index);
            this.userFriendlyColumnName = userFriendlyColumnName;
        }

        public string GetField(IDataRecord record, int recordIndex)
        {
            try
            {
                return inner.GetField(record, recordIndex).ToString();
            }
            catch (Exception e)
            {
                var message = String.Format("Could not read column {0}: {1}", userFriendlyColumnName, e.Message);
                throw new SqlDataFormatUnexpectedException(message, e);                
            }
            
        }
    }
}