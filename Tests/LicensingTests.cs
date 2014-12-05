using System.Collections.Generic;
using Licensing;
using NUnit.Framework;
using Tests.Mocks;

namespace Tests
{
    [TestFixture]
    public class LicensingTests
    {
        [Test]
        public void WhenNoLicenceKeyExistsProductIsUnlicensed()
        {
            //given a registry with no licence key stored in it
            var licenceReader = new LicenceReader(new MockRegistryReader());
            //when we retrieve the licence
            var licence = licenceReader.GetLicence();
            //then the licence will say it's not been fully licensed yet
            Assert.AreEqual(false, licence.IsFullyLicensed, "When there is no licence key in the registry, the product should not be fully licensed");
        }
        
        [Test]
        public void WhenLicenceKeyExistsProductIsLicensed()
        {
            //given a registry with no licence key stored in it
            var licenceReader = new LicenceReader(new MockRegistryReader().SetValueNames("SOFTWARE\\Audition\\Audition", new Dictionary<string, string> {{"LicenceKey", "I am a licence Key"}}));
            //when we retrieve the licence
            var licence = licenceReader.GetLicence();
            //then the licence will say it's not been fully licensed yet
            Assert.AreEqual(true, licence.IsFullyLicensed, "When there is a licence key in the registry, the product should be fully licensed");
        }
    }
}
