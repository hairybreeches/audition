using System;
using System.IO;

namespace Native
{
    public class FileSystem : IFileSystem
    {
        public StreamWriter OpenFileToWrite(string filename)
        {
            return new StreamWriter(filename);
        }

        public StreamReader OpenFileToRead(string filename)
        {
            return new StreamReader(filename);
        }
        
        public bool DirectoryExists(string directoryName)
        {
            return Directory.Exists(directoryName);
        }

        public void EnsureDirectoryExists(string directory)
        {
            if (!DirectoryExists(directory))
            {
                CreateDirectory(directory);
            }
        }

        public bool FileExists(string filename)
        {
            return File.Exists(filename);
        }

        public Stream OpenFileStreamToRead(string filename)
        {
            return File.Open(filename, FileMode.Open, FileAccess.Read);
        }

        private void CreateDirectory(string directory)
        {
            Directory.CreateDirectory(directory);
        }

        public void DeleteFile(string filename)
        {
            if (FileExists(filename))
            {
                File.Delete(filename);
            }
        }
    }
}