using System.Collections.Generic;
using System.Web.Http;
using UserData;

namespace Webapp.Controllers
{
    public class UserDataController : ApiController
    {
        private readonly UserDetails userDetails;

        public UserDataController(UserDetails userDetails)
        {
            this.userDetails = userDetails;
        }

        [HttpGet]
        [Route(Routing.Sage50DataLocations)]        
        public IEnumerable<string> SageDataLocations()
        {
            return userDetails.Sage50DataLocations;
        }
    }
}
