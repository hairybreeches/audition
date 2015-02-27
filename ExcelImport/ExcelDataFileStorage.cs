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

        private static IEnumerable<string> GetDemoDataLocations()
        {
            return new[]
            {
                Path.GetFullPath(".\\ExampleSage50Export.xlsx"),
                Path.GetFullPath(".\\ComplexSage50Export.xls"),
            };
        }        

        public void StoreUsage(ExcelImportMapping mapping)
        {
            var details = GetUserDetails();
            details.AddExcelFile(mapping.SheetDescription.Filename);
            userDetailsStorage.Save(details);
        }

        public IEnumerable<string> GetExcelDataFiles()
        {
            return GetUserDetails().ExcelFiles
                .Concat(GetDemoDataLocations())                
                .Distinct(StringComparer.CurrentCultureIgnoreCase);
        }
    }
}