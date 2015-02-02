using System;
using System.Data;
using System.IO;
using Excel;
using Native;
using SqlImport;

namespace ExcelImport
{
    public class ExcelDataConverter
    {
        private readonly IFileSystem fileSystem;

        public ExcelDataConverter(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public DataTable GetSheet(SheetMetadata data)
        {
            var dataSet = GetDataSet(data.Filename, data.UseHeaderRow);
            var sheet = dataSet.Tables[data.Sheet];
            return sheet;
        }

        public DataReader ReadSheet(SheetMetadata data)
        {
            var sheet = GetSheet(data);
            var firstRow = data.UseHeaderRow ? 2 : 1;
            return new DataReader(sheet.CreateDataReader(), firstRow);
        }

        public DataSet GetDataSet(string filename, bool headerRow = false)
        {
            var reader = GetReader(filename);
            reader.IsFirstRowAsColumnNames = headerRow;
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

            throw new CouldNotOpenExcelFileException(String.Format("Could not recognise Excel file type: '{0}'", extension));
        }

        private Stream OpenFile(string filename)
        {
            return fileSystem.OpenFileStreamToRead(filename);
        }     
    }
}