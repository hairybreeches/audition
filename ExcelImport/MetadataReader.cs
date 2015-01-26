using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Excel;
using Native;

namespace ExcelImport
{
    public class MetadataReader
    {
        private readonly IFileSystem fileSystem;
        private readonly ExcelColumnNamer columnNamer;

        public MetadataReader(IFileSystem fileSystem, ExcelColumnNamer columnNamer)
        {
            this.fileSystem = fileSystem;
            this.columnNamer = columnNamer;
        }

        public IEnumerable<string> ReadSheets(string filename)
        {
            var dataSet = GetDataSet(filename);
            return dataSet.Tables.OfType<DataTable>().Select(x => x.TableName);
        }

        public IEnumerable<string> ReadHeaders(HeaderRowData data)
        {
            var dataSet = GetDataSet(data.Filename, data.UseHeaderRow);
            return data.UseHeaderRow ? GetHeaderRowColumnNames(dataSet) : GetExcelColumnNames(dataSet);
        }

        private DataSet GetDataSet(string filename, bool headerRow = false)
        {
            var reader = GetReader(filename);
            reader.IsFirstRowAsColumnNames = headerRow;
            return reader.AsDataSet();
        }

        private IEnumerable<string> GetHeaderRowColumnNames(DataSet dataSet)
        {            
            return dataSet.Tables[0].Columns.OfType<DataColumn>().Select(x => x.ColumnName);
        }

        private IEnumerable<string> GetExcelColumnNames(DataSet dataSet)
        {            
            return Enumerable.Range(0, dataSet.Tables[0].Columns.Count)
                .Select(x => columnNamer.GetColumnName(x));
        }

        private IExcelDataReader GetReader(string filename)
        {
            if (!fileSystem.FileExists(filename))
            {
                throw new CouldNotOpenExcelFileException(String.Format("The file '{0}' does not exist", filename));
            }

            var extension = Path.GetExtension(filename);
            if (".xls".Equals(extension, StringComparison.CurrentCultureIgnoreCase))
            {
                return ExcelReaderFactory.CreateBinaryReader(OpenFile(filename));
            }
            
            if (".xlsx".Equals(extension, StringComparison.CurrentCultureIgnoreCase))
            {
                return ExcelReaderFactory.CreateOpenXmlReader(OpenFile(filename));
            }

            throw new CouldNotOpenExcelFileException(String.Format("Could not recognise Excel file type: '{0}'", extension));
        }

        private Stream OpenFile(string filename)
        {
            return fileSystem.OpenFileStreamToRead(filename);
        }

        
    }
}
