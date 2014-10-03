using System.IO;

namespace Excel
{
    public class FileSystem : IFileSystem
    {
        public StreamWriter OpenFileToWrite(string filename)
        {
            return new StreamWriter(filename);
        }
    }
}