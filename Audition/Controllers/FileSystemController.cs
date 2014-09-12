using System.Diagnostics;
using System.Web.Http;
using Audition.Chromium;

namespace Audition.Controllers
{
    public class FileSystemController : RedirectController
    {
        [HttpGet]
        [Route(Routing.Openfile)]
        public IHttpActionResult OpenFile(string fileName)
        {
            Process.Start(fileName);
            return Ok();
        }

        
    }
}
