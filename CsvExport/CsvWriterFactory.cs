using CsvHelper;
using Native.Disk;

namespace CsvExport
{
    public class CsvWriterFactory : ISpreadsheetWriterFactory
    {
        private readonly IFileSystem fileSystem;

        public CsvWriterFactory(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public ISpreadsheetWriter CreateWriter(string filename)
        {
            return new CsvWriterWrapper(new CsvWriter(fileSystem.OpenFileToWrite(filename)));
        }        
    }
}