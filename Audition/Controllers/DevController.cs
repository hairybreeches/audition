using System.Web.Http;
using Audition.Chromium;

namespace Audition.Controllers
{
    public class DevController : ApiController
    {
        private readonly ChromiumControl chromiumControl;

        public DevController(ChromiumControl chromiumControl)
        {
            this.chromiumControl = chromiumControl;
        }

        [HttpGet]
        [Route(Routing.ShowDevTools)]
        public void ShowDevTools()
        {
            chromiumControl.ShowDevTools();
        }
    }
}
