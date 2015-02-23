using System;
using System.IO;

namespace Native.Disk
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
            return File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
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
        public static string GetTempFilePathWithExtension(string extension)
        {
            var path = Path.GetTempPath();
            var fileName = Guid.NewGuid() + "." + extension;
            return Path.Combine(path, fileName);
        }

        public TempFile GetTempFile(string extension = "tmp")
        {
            return new TempFile(this, GetTempFilePathWithExtension(extension));
        }
    }
}