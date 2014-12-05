using System.Collections.Generic;
using Licensing;
using Native;
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
            var licenceReader = GetLicenceStorage(new MockRegistry());
            //when we retrieve the licence
            var licence = licenceReader.GetLicence();
            //then the licence will say it's not been fully licensed yet
            Assert.AreEqual(false, licence.IsFullyLicensed, "When there is no licence key in the registry, the product should not be fully licensed");
        }       

        [Test]
        public void WhenLicenceKeyExistsProductIsLicensed()
        {
            //given a registry with a licence key stored in it
            var licenceReader = GetLicenceStorage(new MockRegistry().SetValue("SOFTWARE\\Audition\\Audition", "LicenceKey", "1234567890123456"));
            //when we retrieve the licence
            var licence = licenceReader.GetLicence();
            //then the licence will say it's been fully licensed
            Assert.AreEqual(true, licence.IsFullyLicensed, "When there is a licence key in the registry, the product should be fully licensed");
        }     
        
        [TestCase("123456789012345", ExpectedException = typeof(InvalidLicenceKeyException), TestName = "Licence key with fewer than 16 digits fails")]
        [TestCase("12345678901234567", ExpectedException = typeof(InvalidLicenceKeyException), TestName = "Licence key with more than 16 digits fails")]
        public void LicenceVerificationWorks(string licenceKey)
        {
            GetLicenceStorage(new MockRegistry()).StoreLicence(licenceKey);
        }    
        
        [Test]
        public void CanWriteAndRetrieveLicenceKey()
        {
            //given a registry with no licence key stored in it
            var licenceReader = GetLicenceStorage(new MockRegistry());
            //when we write a new licence and then retrieve the licence
            licenceReader.StoreLicence("1234567890123456");
            var licence = licenceReader.GetLicence();
            //then the licence will say it's been fully licensed now
            Assert.AreEqual(true, licence.IsFullyLicensed, "When there is a licence key in the registry, the product should be fully licensed");
        }

        private static LicenceStorage GetLicenceStorage(ICurrentUserRegistry registry)
        {
            return new LicenceStorage(registry, new LicenceVerifier());
        }
    }
}
