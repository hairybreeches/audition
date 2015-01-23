using System.Collections.Generic;
using System.Web.Http;
using ExcelImport;
using Sage50;

namespace Webapp.Controllers
{
    public class UserDataController : ApiController
    {
        private readonly Sage50DataDirectoryStorage dataDirectoryStorage;
        private readonly ExcelDataFileStorage excelDataFileStorage;

        public UserDataController(Sage50DataDirectoryStorage dataDirectoryStorage, ExcelDataFileStorage excelDataFileStorage)
        {
            this.dataDirectoryStorage = dataDirectoryStorage;
            this.excelDataFileStorage = excelDataFileStorage;
        }

        [HttpGet]
        [Route(Routing.Sage50DataLocations)]        
        public IEnumerable<string> SageDataLocations()
        {            
            return dataDirectoryStorage.GetSageDataDirectories();
        }        
        
        [HttpGet]
        [Route(Routing.ExcelDataFiles)]        
        public IEnumerable<string> ExcelDataFiles()
        {            
            return excelDataFileStorage.GetExcelDataFiles();
        }
    }
}
