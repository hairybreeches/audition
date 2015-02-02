using System;
using System.Data;

namespace SqlImport.DataReaders
{
    public class ToStringDataReader : ISqlDataReader<string>
    {
        private readonly int index;
        private readonly string userFriendlyColumnName;

        public ToStringDataReader(int index, string userFriendlyColumnName)
        {
            this.index = index;
            this.userFriendlyColumnName = userFriendlyColumnName;
        }

        public string GetField(IDataRecord record, int recordIndex)
        {
            try
            {
                return record.GetValue(index).ToString();
            }
            catch (Exception e)
            {
                var message = String.Format("Could not read column {0}: {1}", userFriendlyColumnName, e.Message);
                throw new SqlDataFormatUnexpectedException(message, e);                
            }
            
        }
    }
}