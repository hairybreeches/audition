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
        private readonly IUserDetailsStorage userDetailsStorage;
        private readonly SageDemoDirectorySupplier demoDirectorySupplier;

        public Sage50DataDirectoryStorage(IUserDetailsStorage userDetailsStorage, SageDemoDirectorySupplier demoDirectorySupplier)
        {
            this.userDetailsStorage = userDetailsStorage;
            this.demoDirectorySupplier = demoDirectorySupplier;
        }

        public IEnumerable<string> GetSageDataDirectories()
        {
            return GetUserDetails().Sage50DataLocations.Concat(demoDirectorySupplier.GetDemoDataLocations())
                .Where(x => !String.IsNullOrEmpty(x))
                .Distinct(StringComparer.InvariantCultureIgnoreCase);
        }

        public UserDetails GetUserDetails()
        {
             return userDetailsStorage.Load();
        }        

        public void AddSage50DataLocation(string dataDirectory)
        {
            var userDetails = GetUserDetails();
            userDetails.AddSage50DataLocation(dataDirectory);
            userDetailsStorage.Save(userDetails);
        }
    }
}
