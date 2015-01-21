using System.Web.Http;
using ExcelImport;

namespace Webapp.Controllers
{
    public class ExcelSessionController : ApiController
    {
        [Route(Routing.ExcelLogin)]
        [HttpPost]
        public void ExcelLogin(ExcelImportMapping importMapping)
        {
            
        }
    }
}
