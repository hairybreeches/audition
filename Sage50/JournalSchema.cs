using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Sage50.Parsing;

namespace Sage50
{
    public class JournalSchema
    {
        private readonly SchemaColumn<int> idColumn;
        private readonly SchemaColumn<string> usernameColumn;
        private readonly SchemaColumn<DateTime> dateColumn;
        private readonly SchemaColumn<DateTime> creationTimeColumn;
        private readonly SchemaColumn<string> nominalCodeColumn;
        private readonly SchemaColumn<double> amountColumn;
        private readonly SchemaColumn<string> detailsColumn;

        public JournalSchema()
        {
            idColumn = new SchemaColumn<Int32>("TRAN_NUMBER", 0);
            usernameColumn = new SchemaColumn<string>("USER_NAME", 1);
            dateColumn = new SchemaColumn<DateTime>("DATE", 2);
            creationTimeColumn = new SchemaColumn<DateTime>("RECORD_CREATE_DATE", 3);
            nominalCodeColumn = new SchemaColumn<string>("NOMINAL_CODE", 4);
            amountColumn = new SchemaColumn<double>("AMOUNT", 5);
            detailsColumn = new SchemaColumn<string>("DETAILS", 6);
            ValidateSchemaDefinition();
        }

        private void ValidateSchemaDefinition()
        {
            var definedColumnNumbers = Columns.Select(x => x.Index).ToList();
            var numberOfColumns = Columns.Count();

            var expectedDefinedColumnNumbers = Enumerable.Range(0, numberOfColumns).ToList();
            if (!definedColumnNumbers.SequenceEqual(expectedDefinedColumnNumbers))
            {
                throw new SageDataFormatUnexpectedException(
                    String.Format("Incorrect column definitions: Column numbers defined: {0}",
                        String.Join(",", definedColumnNumbers)));
            }
        }

        public DataColumn[] DataColumns
        {
            get
            {
                return Columns                    
                    .Select(x => x.ToDataColumn())
                    .ToArray();
            }
        }

        private IEnumerable<ISchemaColumn> Columns
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
                    detailsColumn
                }.OrderBy(x => x.Index);
            }
        }

        public IEnumerable<string> ColumnNames
        {
            get { return Columns.Select(x=>x.FieldName); }
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
    }
}