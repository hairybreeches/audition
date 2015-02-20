using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Native;
using Native.Disk;
using UserData;

namespace Sage50
{
    public class Sage50DataDirectoryStorage
    {
        private readonly IUserDetailsStorage userDetailsStorage;
        private readonly IFileSystem fileSystem;
        private readonly Sage50DriverDetector driverDetector;

        public Sage50DataDirectoryStorage(IUserDetailsStorage userDetailsStorage, IFileSystem fileSystem, Sage50DriverDetector driverDetector)
        {
            this.userDetailsStorage = userDetailsStorage;
            this.fileSystem = fileSystem;
            this.driverDetector = driverDetector;
        }

        public IEnumerable<string> GetSageDataDirectories()
        {
            return GetUserDetails().Sage50DataLocations.Concat(GetExistingDemoDataLocations())
                .Where(x => !String.IsNullOrEmpty(x))
                .Distinct(StringComparer.InvariantCultureIgnoreCase);
        }

        public UserDetails GetUserDetails()
        {
             return userDetailsStorage.Load();
        }

        private IEnumerable<string> GetExistingDemoDataLocations()
        {
            return GetDemoDataLocations()
                .Select(Environment.ExpandEnvironmentVariables)
                .Where(fileSystem.DirectoryExists);
        }


        private IEnumerable<string> GetDemoDataLocations()
        {
            return driverDetector.FindSageDrivers().Select(x => x.GetDemoDataLocation());
        }

        public void AddSage50DataLocation(string dataDirectory)
        {
            var userDetails = GetUserDetails();
            userDetails.AddSage50DataLocation(dataDirectory);
            userDetailsStorage.Save(userDetails);
        }
    }
}
