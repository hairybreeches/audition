using System.Collections.Generic;
using System.Web.Http;
using ExcelImport;
using Model;

namespace Webapp.Controllers
{
    public class ExcelSessionController : ApiController
    {
        private readonly HeaderReader headerReader;
        private readonly ExcelDataFileStorage dataFileStorage;

        public ExcelSessionController(HeaderReader headerReader, ExcelDataFileStorage dataFileStorage)
        {
            this.headerReader = headerReader;
            this.dataFileStorage = dataFileStorage;
        }

        [Route(Routing.ExcelLogin)]
        [HttpPost]
        public void ExcelLogin(ExcelImportMapping importMapping)
        {
            dataFileStorage.StoreUsage(importMapping);
        }    
        
        
        [Route(Routing.GetExcelHeaders)]
        [HttpPost]
        public IEnumerable<string> GetHeaders(HeaderRowData headerRowData)
        {
            return headerReader.ReadHeaders(headerRowData);
        }
    }
}
