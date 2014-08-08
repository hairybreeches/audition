using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Model;

namespace Audition.Controllers
{
    public class SearchController : ApiController
    {
        public IEnumerable<Journal> GetSearch()
        {
            return Enumerable.Empty<Journal>();            
        }
    }
}
