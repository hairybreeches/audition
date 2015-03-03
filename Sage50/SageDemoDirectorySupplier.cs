using System;
using System.Collections.Generic;
using System.Linq;
using Native.Disk;
using UserData;

namespace Sage50
{
    public class SageDemoDirectorySupplier : IDemoDataSupplier
    {
        private readonly IFileSystem fileSystem;
        private readonly Sage50DriverDetector driverDetector;

        public SageDemoDirectorySupplier(IFileSystem fileSystem, Sage50DriverDetector driverDetector)
        {
            this.fileSystem = fileSystem;
            this.driverDetector = driverDetector;
        }

        public IEnumerable<string> GetDemoDataLocations()
        {
            return GetPotentialDemoDataLocations()
                .Select(Environment.ExpandEnvironmentVariables)
                .Where(fileSystem.DirectoryExists);
        }


        private IEnumerable<string> GetPotentialDemoDataLocations()
        {
            return driverDetector.FindSageDrivers().Select(x => x.GetDemoDataLocation());
        }
    }
}