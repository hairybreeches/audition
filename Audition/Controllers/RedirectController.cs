using System.Web.Http;
using System.Web.Http.Results;
using Audition.Chromium;

namespace Audition.Controllers
{
    public class RedirectController : ApiController
    {
        protected RedirectResult RedirectToView(string viewName)
        {
            return base.Redirect(Routing.GetViewUrl(viewName));
        }
    }
}
