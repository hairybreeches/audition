using System.Diagnostics;
using System.Web.Http;

namespace Audition.Controllers
{
    public class FileSystemController : RedirectController
    {
        [HttpGet]
        [Route("api/openfile")]
        public IHttpActionResult OpenFile(string fileName)
        {
            Process.Start(fileName);
            return Ok();
        }

        
    }
}
