using System.Collections.Generic;
using System.Linq;
using Native;
using NSubstitute;
using NUnit.Framework;
using Sage50;
using Tests.Mocks;

namespace Tests
{
    [TestFixture]
    public class DriverDetectorTests
    {
        public IEnumerable<TestCaseData> DriverTestCases 
        {
            get
            {
                yield return new TestCaseData(new List<string> {"Sage Line 50 v21"})
                    .Returns(new[]{new Sage50Driver(21, "Sage Line 50 v21", "2015")})
                    .SetName("When only one driver present it is returned");

                yield return new TestCaseData(new List<string> { "Sage Line 50 v11", "Sage Line 50 v13", "Sage Line 50 v22", "Sage Line 50 v33" })
                    .Returns(new[]
                    {
                        new Sage50Driver(33, "Sage Line 50 v33", "Sage Line 50 v33"), 
                        new Sage50Driver(22, "Sage Line 50 v22", "Sage Line 50 v22"), 
                        new Sage50Driver(13, "Sage Line 50 v13", "Sage Line 50 v13"), 
                        new Sage50Driver(11, "Sage Line 50 v11", "Sage Line 50 v11")
                    })
                    .SetName("Returns highest version installed");

                yield return new TestCaseData(new List<string> {"Sage Line 50 v21", "Sage Line 50 v3"})
                    .Returns(new[]
                    {
                        new Sage50Driver(21, "Sage Line 50 v21", "2015"),
                        new Sage50Driver(3, "Sage Line 50 v3", "Sage Line 50 v3"),
                    }
                    )
                    .SetName("Version numbers are sorted as ints, not strings");

                yield return new TestCaseData(new List<string> { "Sage Line 50 v16", "Sage Line 50 v11", "SQL Server" })
                    .Returns(new []
                    {
                     new Sage50Driver(16, "Sage Line 50 v16", "2010"),
                     new Sage50Driver(11, "Sage Line 50 v11", "Sage Line 50 v11"),      
                    })
                    .SetName("Non-Sage drivers are ignored");

                yield return new TestCaseData(new List<string> { "Sage Line 50 v26", "Sage Line 50 vichysoisse add-in", "Sage Line 50 v11", "SQL Server" })
                    .Returns(new[]
                    {
                     new Sage50Driver(26, "Sage Line 50 v26", "Sage Line 50 v26"),   
                     new Sage50Driver(11, "Sage Line 50 v11", "Sage Line 50 v11"),   
                    })
                    .SetName("Unparseable 'Sage' drivers are ignored");

                yield return new TestCaseData(new List<string> { "SQL Server", "Odbc Connector Steve", "I'm still not a Sage connector" })
                    .Throws(typeof(SageNotInstalledException))
                    .SetName("If Sage isn't installed, we get the right type of error");
                
            }
        }        

        [TestCaseSource("DriverTestCases")]
        public IEnumerable<Sage50Driver> FindSageDriver(IEnumerable<string> drivers)
        {
            var registry = new MockRegistry().SetSage50Drivers(drivers.ToArray());
            return new Sage50DriverDetector(new OdbcRegistryReader(registry)).FindSageDrivers();
        }
    }
}
