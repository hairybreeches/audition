using System;
using System.Collections.Generic;
using System.Linq;

namespace UserData
{
    public class DataLocationStorage
    {
        private readonly IUserDetailsStorage userDetailsStorage;
        private readonly IDemoDataSupplier demoDataSupplier;
        private readonly Func<UserDetails, IEnumerable<string>> readData;
        private readonly Func<UserDetails, Action<string>> saveData;

        public DataLocationStorage(IUserDetailsStorage userDetailsStorage, IDemoDataSupplier demoDataSupplier, Func<UserDetails, IEnumerable<string>> readData, Func<UserDetails, Action<string>> saveData)
        {
            this.userDetailsStorage = userDetailsStorage;
            this.demoDataSupplier = demoDataSupplier;
            this.readData = readData;
            this.saveData = saveData;
        }

        public IEnumerable<string> GetLocations()
        {
            return readData(GetUserDetails()).Concat(demoDataSupplier.GetDemoDataLocations())
                .Where(x => !String.IsNullOrEmpty(x))
                .Distinct(StringComparer.InvariantCultureIgnoreCase);
        }

        private UserDetails GetUserDetails()
        {
            return userDetailsStorage.Load();
        }        

        public void AddLocation(string dataDirectory)
        {
            var userDetails = GetUserDetails();
            saveData(userDetails)(dataDirectory);
            userDetailsStorage.Save(userDetails);
        }
    }
}