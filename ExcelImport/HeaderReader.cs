using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using Excel;
using Native;

namespace ExcelImport
{
    public class HeaderReader
    {
        private readonly IFileSystem fileSystem;
        private readonly ExcelColumnNamer columnNamer;

        public HeaderReader(IFileSystem fileSystem, ExcelColumnNamer columnNamer)
        {
            this.fileSystem = fileSystem;
            this.columnNamer = columnNamer;
        }

        public IEnumerable<string> ReadHeaders(HeaderRowData data)
        {
            var reader = GetReader(data.Filename);
            if (data.UseHeaderRow)
            {
                return GetHeaderRowColumnNames(reader);
            }
            else
            {
                return GetExcelColumnNames(reader);                
            }

            
        }

        private IEnumerable<string> GetHeaderRowColumnNames(IExcelDataReader reader)
        {
            reader.IsFirstRowAsColumnNames = true;
            var result = reader.AsDataSet();
            return result.Tables[0].Columns.OfType<DataColumn>().Select(x => x.ColumnName);
        }

        private IEnumerable<string> GetExcelColumnNames(IExcelDataReader reader)
        {
            var result = reader.AsDataSet();
            return Enumerable.Range(0, result.Tables[0].Columns.Count)
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
