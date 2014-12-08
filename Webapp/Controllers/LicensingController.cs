using System.Web.Http;
using Licensing;

namespace Webapp.Controllers
{
    public class LicensingController : ApiController
    {
        private readonly LicenceStorage storage;

        public LicensingController(LicenceStorage storage)
        {
            this.storage = storage;
        }


        [HttpGet]
        [Route(Routing.GetLicence)]
        public ILicence GetLicence()
        {
            return storage.GetLicence();
        }

        [HttpPost]
        [Route(Routing.UpdateLicence)]
        public ILicence UpdateLicence(string licenceKey)
        {
            storage.StoreLicence(licenceKey);
            return GetLicence();
        }
    }
}
