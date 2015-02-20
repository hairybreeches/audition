using CsvHelper;
using Native.Disk;

namespace CsvExport
{
    public class CsvWriterFactory
    {
        private readonly IFileSystem fileSystem;

        public CsvWriterFactory(IFileSystem fileSystem)
        {
            this.fileSystem = fileSystem;
        }

        public ICsvWriter CreateWriter(string filename)
        {
            return new CsvWriter(fileSystem.OpenFileToWrite(filename));
        }        
    }
}