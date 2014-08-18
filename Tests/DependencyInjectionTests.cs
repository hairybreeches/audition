using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Audition;
using Audition.Controllers;
using Autofac;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class DependencyInjectionTests
    {
        //todo: would be nice to check we can make an AppForm here too
        [TestCaseSource("Controllers")]
        public void CanCreateClass(Type type)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<AuditionModule>();
            using (var container = builder.Build())
            {
                var result = container.Resolve(type);
                Assert.NotNull(result);
            }
        }

        public IEnumerable<Type> Controllers
        {
            get
            {
                var controllerAssembly = typeof(SearchController).Assembly;
                return controllerAssembly.GetTypes()
                    .Where(x => x.IsSubclassOf(typeof (ApiController)))
                    .Where(x=> !x.IsAbstract);

            }
        }
    }
}
