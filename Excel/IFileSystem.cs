using System.IO;

namespace Excel
{
    public interface IFileSystem
    {
        StreamWriter OpenFileToWrite(string filename);
    }
}