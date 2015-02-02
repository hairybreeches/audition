using System.Web.Http;
using System.Web.Http.Results;

namespace Webapp.Controllers
{
    public class RedirectController : ApiController
    {
        protected RedirectResult RedirectToView(string viewName)
        {
            return base.Redirect(Routing.GetViewUrl(viewName));
        }
    }
}
