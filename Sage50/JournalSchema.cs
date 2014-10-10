using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Sage50.Parsing;

namespace Sage50
{
    public class JournalSchema
    {
        private readonly DataColumn[] columns =
        {
            new DataColumn("TRAN_NUMBER", typeof (Int32)),
            new DataColumn("USER_NAME", typeof (String)),
            new DataColumn("DATE", typeof (DateTime)),
            new DataColumn("RECORD_CREATE_DATE", typeof (DateTime)),
            new DataColumn("NOMINAL_CODE", typeof (String)),
            new DataColumn("AMOUNT", typeof (double)),
            new DataColumn("DETAILS", typeof (String)),
        };

        public DataColumn[] Columns
        {
            get { return columns; }
        }      
        
        public IEnumerable<string> ColumnNames
        {
            get { return columns.Select(x=>x.ColumnName); }
        }

        public int GetId(IDataRecord record)
        {
            return GetField<int>(record, 0);
        }
        
        public string GetUsername(IDataRecord record)
        {
            return GetField<string>(record, 1);
        }
        
        public DateTime GetJournalDate(IDataRecord record)
        {
            return GetField<DateTime>(record, 2);
        }
        
        public DateTime GetCreationTime(IDataRecord record)
        {
            return GetField<DateTime>(record, 3);
        }
        
        public string GetNominalCode(IDataRecord record)
        {
            return GetField<string>(record, 4);
        }
        
        public double GetAmount(IDataRecord record)
        {
            return GetField<Double>(record, 5);
        }
        
        public string GetDescription(IDataRecord record)
        {
            return GetField<string>(record, 6);
        }        

        private T GetField<T>(IDataRecord record, int index)
        {
            var fieldName = columns[index].ColumnName;

            var actualFieldName = record.GetName(index);
            if (actualFieldName != fieldName)
            {
                throw new SageDataFormatUnexpectedException(
                    String.Format("Unrecognised data schema. Column {0} was {1}, expected {2}", index, actualFieldName, fieldName));
            }

            var fieldValue = record[index];

            if (!(fieldValue is T))
            {
                throw new SageDataFormatUnexpectedException(String.Format("Unrecognised data schema. {0} was {1}, expected {2}", fieldName, fieldValue.GetType(), typeof(T)));
            }

            return (T)fieldValue;
        }
    }
}
