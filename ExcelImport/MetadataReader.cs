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

        public IEnumerable<SheetMetadata> ReadSheets(string filename)
        {
            var dataSet = toSqlDataConverter.GetDataSet(filename, true);
            return dataSet.Tables.OfType<DataTable>().Select(GetMetadata);
        }

        private SheetMetadata GetMetadata(DataTable table)
        {
            var dataColumns = table.Columns.OfType<DataColumn>().ToList();
            return new SheetMetadata(table.TableName, GetExcelColumnNames(dataColumns), GetHeaderRowColumnNames(dataColumns));
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
