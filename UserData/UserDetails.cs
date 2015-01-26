using System.Collections.Generic;
using Model;

namespace UserData
{
    public class UserDetails
    {
        private readonly MostRecentList<string> sageDataLocations = new MostRecentList<string>(20);        
        private readonly MostRecentList<string> excelImportList = new MostRecentList<string>(20);        


        public IEnumerable<string> Sage50DataLocations
        {
            get { return sageDataLocations.GetMostRecentValues(); }
        }


        public void AddSage50DataLocation(string location)
        {
            sageDataLocations.AddUsage(location);
        }    
        
        public void AddExcelFile(string mapping)
        {
            excelImportList.AddUsage(mapping);
        }

        public IEnumerable<string> ExcelFiles
        {
            get { return excelImportList.GetMostRecentValues(); }
        }
    }
}
