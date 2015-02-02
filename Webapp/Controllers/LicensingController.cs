using System.Web.Http;
using Licensing;

namespace Webapp.Controllers
{
    public class LicensingController : ApiController
    {
        private readonly ILicenceStorage storage;

        public LicensingController(ILicenceStorage storage)
        {
            this.storage = storage;
        }


        [HttpGet]
        [Route(Routing.GetLicence)]
        public Licence GetLicence()
        {
            return storage.GetLicence();
        }

        [HttpGet]
        [Route(Routing.UpdateLicence)]
        public Licence UpdateLicence(string licenceKey)
        {
            storage.StoreLicence(licenceKey);
            return GetLicence();
        }
    }
}
