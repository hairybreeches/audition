using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Model;
using UserData;

namespace ExcelImport
{
    public class ExcelDataFileStorage
    {

        private readonly IUserDetailsStorage userDetailsStorage;

        public ExcelDataFileStorage(IUserDetailsStorage userDetailsStorage)
        {
            this.userDetailsStorage = userDetailsStorage;
        }

        
        public UserDetails GetUserDetails()
        {
             return userDetailsStorage.Load();
        }

        private IEnumerable<ExcelImportMapping> GetDemoDataLocations()
        {
            return new[]
            {
                new ExcelImportMapping()
                {
                    Filename = Path.GetFullPath(".\\ExampleSage50Export.xlsx"),
                    Lookups = new FieldLookups(),
                    UseHeaderRow = true
                }
            };
        }        

        public void StoreUsage(ExcelImportMapping mapping)
        {
            var details = GetUserDetails();
            details.AddExcelMapping(mapping);
            userDetailsStorage.Save(details);
        }

        public IEnumerable<string> GetExcelDataFiles()
        {
            return GetUserDetails().ExcelMappings
                .Concat(GetDemoDataLocations())
                .Select(x => x.Filename)
                .Distinct(StringComparer.CurrentCultureIgnoreCase);
        }
    }
}