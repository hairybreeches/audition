using System;
using System.Collections.Generic;
using Licensing;
using Native;
using NSubstitute;
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
        [TestCase("1234567890123456", ExpectedException = typeof(InvalidLicenceKeyException), TestName = "Licence key with invalid checksum fails")]
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
            licenceReader.StoreLicence("123456789012B032");
            var licence = licenceReader.GetLicence();
            //then the licence will say it's been fully licensed now
            Assert.AreEqual(true, licence.IsFullyLicensed, "When there is a licence key in the registry, the product should be fully licensed");
        }

        [Test]
        public void WhenLicenceDoesNotExistTrialPeriodHas28Days()
        {
            //given a blank slate in the regsitry
            var storage = GetLicenceStorage(new MockRegistry(), new DateTime(1999, 1, 1));

            //when we get the licence
            var licence = storage.GetLicence();

            //then there are 28 days left on the trial
            Assert.AreEqual(28, licence.RemainingTrialDays);
            Assert.AreEqual(true, licence.TrialValid);
        }

        [TestCase(0, 28, true)]
        [TestCase(15, 13, true)]        
        [TestCase(28, 0, true)]
        [TestCase(29, 0, false)]
        [TestCase(45, 0, false)]
        public void TrialRunsDownAsExpected(int daysAfterInitialisation, int expectedDaysRemaining, bool trialStillValid)
        {
            //given a blank registry accessed for the first time
            var mockRegistry = new MockRegistry();
            var initialAccessDate = new DateTime(1999, 1, 1);
            var storage = GetLicenceStorage(mockRegistry, initialAccessDate);
            storage.GetLicence();

            //when we get the licence so many days later
            var licence = GetLicenceStorage(mockRegistry, initialAccessDate.AddDays(daysAfterInitialisation)).GetLicence();

            //then there are the right number of days left on the trial
            Assert.AreEqual(expectedDaysRemaining, licence.RemainingTrialDays);
            Assert.AreEqual(trialStillValid, licence.TrialValid);
        }

        private static LicenceStorage GetLicenceStorage(ICurrentUserRegistry registry)
        {
            return GetLicenceStorage(registry, DateTime.MinValue);
        }

        private static LicenceStorage GetLicenceStorage(ICurrentUserRegistry registry, DateTime now)
        {
            var clock = Substitute.For<IClock>();
            clock.GetCurrentDate().Returns(now);
            return new LicenceStorage(registry, new LicenceVerifier(), clock);
        }
    }
}
