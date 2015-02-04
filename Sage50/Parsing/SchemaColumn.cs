using System;
using System.Data;
using SqlImport;
using SqlImport.DataReaders;

namespace Sage50.Parsing
{
    public class SchemaColumn<T> : ISchemaColumn
    {
        private readonly int index;
        private readonly IFieldReader<T> dataReader;
        private readonly string fieldName;


        public SchemaColumn(string fieldName, int index)
            :this(fieldName, index, (name, i) => new SqlDataReader<T>(i, name))
        {
        }

        public SchemaColumn(string fieldName, int index, Func<string, int, IFieldReader<T>> dataReaderFactory)
        {
            this.fieldName = fieldName;
            this.index = index;
            this.dataReader = new ColumnNameVerifierDecorator<T>(fieldName, index, dataReaderFactory(fieldName, index));
        }

        public int Index
        {
            get { return index; }
        }

        public string FieldName
        {
            get { return fieldName; }
        }

        public IFieldReader<T> DataReader
        {
            get { return dataReader; }
        }

        public DataColumn ToDataColumn()
        {
            return new DataColumn(FieldName, typeof(T));
        }                
    }
}