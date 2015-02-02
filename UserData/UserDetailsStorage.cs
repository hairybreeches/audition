using System;
using System.IO;
using Native;
using Newtonsoft.Json;

namespace UserData
{
    public class UserDetailsStorage : IUserDetailsStorage
    {
        private readonly IFileSystem fileSystem;
        private readonly string filename;
        private const string DefaultFilename = "%APPDATA%\\Audition\\preferences.dat";

        public UserDetailsStorage(IFileSystem fileSystem)
            :this(fileSystem, Environment.ExpandEnvironmentVariables(DefaultFilename))
        {
        }
        
        public UserDetailsStorage(IFileSystem fileSystem, string filename)
        {
            this.fileSystem = fileSystem;
            this.filename = filename;
        }

        public void Save(UserDetails userDetails)
        {
            var directory = Path.GetDirectoryName(filename);
            fileSystem.EnsureDirectoryExists(directory);

            using (var writer = fileSystem.OpenFileToWrite(filename))
            {
                writer.Write(JsonConvert.SerializeObject(userDetails));
            }
        }
        
        public UserDetails Load()
        {
            if(fileSystem.FileExists(filename))
            {

                using (var reader = fileSystem.OpenFileToRead(filename))
                {
                    return JsonConvert.DeserializeObject<UserDetails>(reader.ReadToEnd()) ?? new UserDetails();
                }
            }
            else
            {
                return new UserDetails();
            }            
        }
    }
}