using System;
using System.IO;

namespace Native.Disk
{
    public class TempFile : IDisposable
    {
        private readonly IFileSystem fileSystem;
        private readonly string filename;

        public TempFile(IFileSystem fileSystem, string filename)
        {
            this.fileSystem = fileSystem;
            this.filename = filename;
        }

        public string Filename
        {
            get { return filename; }
        }

        public StreamReader OpenFileToRead()
        {
            return fileSystem.OpenFileToRead(filename);
        }

        public StreamWriter OpenFileToWrite()
        {
            return fileSystem.OpenFileToWrite(filename);
        }

        public void Dispose()
        {
            fileSystem.DeleteFile(filename);
        }
    }
}