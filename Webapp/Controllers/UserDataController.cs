using System.Collections.Generic;
using System.Web.Http;
using Sage50;

namespace Webapp.Controllers
{
    public class UserDataController : ApiController
    {
        private readonly Sage50DataDirectoryStorage dataDirectoryStorage;

        public UserDataController(Sage50DataDirectoryStorage dataDirectoryStorage)
        {
            this.dataDirectoryStorage = dataDirectoryStorage;
        }

        [HttpGet]
        [Route(Routing.Sage50DataLocations)]        
        public IEnumerable<string> SageDataLocations()
        {            
            return dataDirectoryStorage.GetSageDataDirectories();
        }
    }
}
