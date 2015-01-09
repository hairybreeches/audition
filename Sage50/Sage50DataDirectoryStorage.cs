using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Native;
using UserData;

namespace Sage50
{
    public class Sage50DataDirectoryStorage
    {
        private readonly UserDetails userDetails;
        private readonly IFileSystem fileSystem;
        private readonly Sage50DriverDetector driverDetector;

        public Sage50DataDirectoryStorage(UserDetails userDetails, IFileSystem fileSystem, Sage50DriverDetector driverDetector)
        {
            this.userDetails = userDetails;
            this.fileSystem = fileSystem;
            this.driverDetector = driverDetector;
        }

        public IEnumerable<string> GetSageDataDirectories()
        {
            return userDetails.Sage50DataLocations.Concat(GetExistingDemoDataLocations());
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
            userDetails.AddSage50DataLocation(dataDirectory);
        }
    }
}
