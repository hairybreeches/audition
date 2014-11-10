using System.IO;

namespace Native
{
    public class FileSystem : IFileSystem
    {
        public StreamWriter OpenFileToWrite(string filename)
        {
            return new StreamWriter(filename);
        }
    }
}