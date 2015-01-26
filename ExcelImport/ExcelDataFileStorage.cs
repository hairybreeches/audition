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

        private static IEnumerable<ExcelImportMapping> GetDemoDataLocations()
        {
            return new[]
            {
                new ExcelImportMapping
                {
                    SheetData = new HeaderRowData
                    {
                        Filename = Path.GetFullPath(".\\ExampleSage50Export.xlsx"),
                        UseHeaderRow = true,
                        Sheet = 0
                    },

                    Lookups = new FieldLookups()
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
                .Select(x => x.SheetData.Filename)
                .Distinct(StringComparer.CurrentCultureIgnoreCase);
        }
    }
}