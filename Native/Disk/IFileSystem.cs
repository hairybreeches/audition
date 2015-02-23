using System.IO;

namespace Native.Disk
{
    public interface IFileSystem
    {
        StreamWriter OpenFileToWrite(string filename);
        StreamReader OpenFileToRead(string filename);
        void DeleteFile(string filename);
        bool DirectoryExists(string directoryName);
        void EnsureDirectoryExists(string directory);
        bool FileExists(string filename);
        Stream OpenFileStreamToRead(string filename);
        TempFile GetTempFile();
    }
}