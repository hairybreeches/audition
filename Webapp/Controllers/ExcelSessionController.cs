using System.Collections.Generic;
using System.Web.Http;
using ExcelImport;

namespace Webapp.Controllers
{
    public class ExcelSessionController : ApiController
    {
        private readonly HeaderReader headerReader;

        public ExcelSessionController(HeaderReader headerReader)
        {
            this.headerReader = headerReader;
        }

        [Route(Routing.ExcelLogin)]
        [HttpPost]
        public void ExcelLogin(ExcelImportMapping importMapping)
        {
            
        }    
        
        
        [Route(Routing.GetExcelHeaders)]
        [HttpPost]
        public IEnumerable<string> GetHeaders(HeaderRowData headerRowData)
        {
            return headerReader.ReadHeaders(headerRowData);
        }
    }
}
