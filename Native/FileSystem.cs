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

        public void DeleteFile(string filename)
        {
            try
            {
                File.Delete(filename);
            }
            catch (FileNotFoundException)
            {                                
            }            
        }
    }
}