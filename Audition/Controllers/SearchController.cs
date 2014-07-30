using System;
using System.Collections.Generic;
using System.Web.Http;
using Model;

namespace Audition.Controllers
{
    public class SearchController : ApiController
    {
        public IEnumerable<Journal> GetSearch()
        {
            return new[]
            {
                new Journal(Guid.NewGuid(), new DateTime(1985, 12, 27), new DateTime(1957, 5, 4)),
                new Journal(Guid.NewGuid(), new DateTime(1999, 12, 31), new DateTime(1789, 7, 14)),
                new Journal(Guid.NewGuid(), new DateTime(2014, 7, 30), new DateTime(1649, 1, 30))
            };
        }
    }
}
