using System;
using System.Data;
using System.IO;
using Excel;
using Native;
using SqlImport;

namespace ExcelImport
{
    public class ExcelToSqlDataConverter
    {
        private readonly IFileSystem fileSystem;

        public ExcelToSqlDataConverter(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public DataTable GetSheet(DataSet sheetDescription)
        {
            var dataSet = GetDataSet(sheetDescription.Filename, sheetDescription.UseHeaderRow);
            var sheet = dataSet.Tables[sheetDescription.Sheet];
            return sheet;
        }

        public SqlDataReader ReadSheet(DataSet sheetDescription)
        {
            var sheet = GetSheet(sheetDescription);
            var firstRow = sheetDescription.UseHeaderRow ? 2 : 1;
            return new SqlDataReader(sheet.CreateDataReader(), firstRow);
        }

        public System.Data.DataSet GetDataSet(string filename, bool useHeaderRow)
        {
            var reader = GetReader(filename);
            reader.IsFirstRowAsColumnNames = useHeaderRow;
            return reader.AsDataSet();
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

            throw new CouldNotOpenExcelFileException(String.Format("Could not recognise Excel file extension: '{0}'", extension));
        }

        private Stream OpenFile(string filename)
        {
            return fileSystem.OpenFileStreamToRead(filename);
        }     
    }
}