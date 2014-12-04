using System;
using System.IO;

namespace Native
{
    public interface IFileSystem
    {
        StreamWriter OpenFileToWrite(string filename);
        StreamReader OpenFileToRead(string filename);
        void DeleteFile(string filename);
        bool DirectoryExists(string directoryName);
    }
}