using System.Collections.Generic;
using Model;

namespace UserData
{
    public class UserDetails
    {
        private readonly MostRecentList<string> sageDataLocations = new MostRecentList<string>(20);        
        private readonly MostRecentList<ExcelImportMapping> excelImportList = new MostRecentList<ExcelImportMapping>(20);        


        public IEnumerable<string> Sage50DataLocations
        {
            get { return sageDataLocations.GetMostRecentValues(); }
        }


        public void AddSage50DataLocation(string location)
        {
            sageDataLocations.AddUsage(location);
        }    
        
        public void AddExcelMapping(ExcelImportMapping mapping)
        {
            excelImportList.AddUsage(mapping);
        }

        public IEnumerable<ExcelImportMapping> ExcelMappings
        {
            get { return excelImportList.GetMostRecentValues(); }
        }
    }
}
