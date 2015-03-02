using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using UserData;

namespace ExcelImport
{
    public class ExcelDataFileStorage
    {

        private readonly IUserDetailsStorage userDetailsStorage;
        private readonly ExcelDemoDataSupplier demoDataSupplier;

        public ExcelDataFileStorage(IUserDetailsStorage userDetailsStorage, ExcelDemoDataSupplier demoDataSupplier)
        {
            this.userDetailsStorage = userDetailsStorage;
            this.demoDataSupplier = demoDataSupplier;
        }

        public UserDetails GetUserDetails()
        {
             return userDetailsStorage.Load();
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
                .Concat(demoDataSupplier.GetDemoDataLocations())                
                .Distinct(StringComparer.CurrentCultureIgnoreCase);
        }
    }
}