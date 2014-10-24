using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using Sage50;

namespace Tests
{
    [TestFixture]
    public class DriverDetectorTests
    {
        public IEnumerable<TestCaseData> DriverTestCases 
        {
            get
            {
                yield return new TestCaseData(new List<string> {"Sage Line 50 v21"}).Returns(new Sage50Driver(21, "Sage Line 50 v21"))
                    .SetName("When only one driver present it is returned");
                yield return new TestCaseData(new List<string> { "Sage Line 50 v11", "Sage Line 50 v13", "Sage Line 50 v22", "Sage Line 50 v33" }).Returns(new Sage50Driver(33, "Sage Line 50 v33"))
                    .SetName("Returns highest version installed");
                yield return new TestCaseData(new List<string> {"Sage Line 50 v21", "Sage Line 50 v3"}).Returns(new Sage50Driver(21, "Sage Line 50 v21"))
                    .SetName("Version numbers are sorted as ints, not strings");
                
            }
        }        

        [TestCaseSource("DriverTestCases")]
        public Sage50Driver FindSageDriver(IEnumerable<string> drivers)
        {
            var registry = CreateRegistry(drivers);
            var driver = new Sage50DriverDetector(registry).FindBestDriver();
            return driver;
        }

        private static IRegistryReader CreateRegistry(IEnumerable<string> drivers)
        {
            var registry = Substitute.For<IRegistryReader>();
            registry.Get32BitOdbcDrivers().Returns(drivers);
            return registry;
        }
    }
}
