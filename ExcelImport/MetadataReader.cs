using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace ExcelImport
{
    public class MetadataReader
    {        
        private readonly ExcelColumnNamer columnNamer;
        private readonly ExcelToSqlDataConverter toSqlDataConverter;

        public MetadataReader(ExcelColumnNamer columnNamer, ExcelToSqlDataConverter toSqlDataConverter)
        {            
            this.columnNamer = columnNamer;
            this.toSqlDataConverter = toSqlDataConverter;
        }

        public IEnumerable<string> ReadSheets(string filename)
        {
            var dataSet = toSqlDataConverter.GetDataSet(filename);
            return dataSet.Tables.OfType<DataTable>().Select(x => x.TableName);
        }

        public IEnumerable<string> ReadHeaders(SheetMetadata data)
        {
            var sheet = toSqlDataConverter.GetSheet(data);
            var dataColumns = sheet.Columns.OfType<DataColumn>();
            return data.UseHeaderRow ? GetHeaderRowColumnNames(dataColumns) : GetExcelColumnNames(dataColumns);
        }

        private static IEnumerable<string> GetHeaderRowColumnNames(IEnumerable<DataColumn> dataColumns)
        {            
            return dataColumns.Select(x => x.ColumnName);
        }

        private IEnumerable<string> GetExcelColumnNames(IEnumerable<DataColumn> dataColumns)
        {            
            return Enumerable.Range(0, dataColumns.Count())
                .Select(x => "Column " + columnNamer.GetColumnName(x));
        }

     
    }
}
