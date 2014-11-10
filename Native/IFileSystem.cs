using System.IO;

namespace Native
{
    public interface IFileSystem
    {
        StreamWriter OpenFileToWrite(string filename);
    }
}