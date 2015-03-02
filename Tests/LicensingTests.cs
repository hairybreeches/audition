using System;
using System.Collections.Generic;
using Licensing;
using Native;
using Native.RegistryAccess;
using Native.Time;
using NSubstitute;
using NUnit.Framework;
using Tests.Mocks;

namespace Tests
{
    [TestFixture]
    public class LicensingTests
    {
        private const string ValidLicenceKey = "123456789012B032";

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
            var licenceReader = GetLicenceStorage(new MockRegistry().SetLicenceKey(ValidLicenceKey));
            //when we retrieve the licence
            var licence = licenceReader.GetLicence();
            //then the licence will say it's been fully licensed
            Assert.AreEqual(true, licence.IsFullyLicensed, "When there is a licence key in the registry, the product should be fully licensed");
        }

        [TestCaseSource("LicenceTestCases")]
        [TestCase(ValidLicenceKey, TestName = "Basic success case")]
        [TestCase("  " + ValidLicenceKey + "    ", TestName = "Key with leading and trailing whitespace")]        
        [TestCase("123456789012345", ExpectedException = typeof(InvalidLicenceKeyException), TestName = "Licence key with fewer than 16 digits fails")]
        [TestCase("12345678901234567", ExpectedException = typeof(InvalidLicenceKeyException), TestName = "Licence key with more than 16 digits fails")]
        [TestCase("1234567890123456", ExpectedException = typeof(InvalidLicenceKeyException), TestName = "Licence key with invalid checksum fails")]
        public void CanWriteAndRetrieveLicenceKeyWhenValid(string licenceKey)
        {
            //given a registry with no licence key stored in it
            var licenceReader = GetLicenceStorage(new MockRegistry());
            //when we write a new licence and then retrieve the licence
            licenceReader.StoreLicence(licenceKey);
            var licence = licenceReader.GetLicence();
            //then the licence will say it's been fully licensed now
            Assert.AreEqual(true, licence.IsFullyLicensed, "When there is a licence key in the registry, the product should be fully licensed");
        }

        public IEnumerable<TestCaseData> LicenceTestCases
        {
            get
            {
                yield return new TestCaseData(ValidLicenceKey.ToLower()).SetName("Can licence with lowercase valid key");
                yield return new TestCaseData(ValidLicenceKey.ToUpper()).SetName("Can licence with uppercase valid key");
            }
        }
            
            
            [Test]
        public void WhenInvalidLicenceStoredItIsIgnored()
        {
            //given a registry with an invalid licence key
            var registry = new MockRegistry().SetLicenceKey("1234567890123456");
            var licence = GetLicenceStorage(registry).GetLicence();
            Assert.AreEqual(false, licence.IsFullyLicensed, "The false licence should be ignored or removed");

        }

        [Test]
        public void WhenLicenceDoesNotExistTrialPeriodHas28Days()
        {
            //given a blank slate in the regsitry
            var storage = GetLicenceStorage(new MockRegistry(), new DateTime(1999, 1, 1));

            //when we get the licence
            var licence = storage.GetLicence();

            //then there are 28 days left on the trial
            Assert.AreEqual(14, licence.RemainingTrialDays);
            Assert.AreEqual(true, licence.TrialValid);
        }

        [TestCase(0, 14, true)]
        [TestCase(6, 8, true)]        
        [TestCase(14, 0, true)]
        [TestCase(15, 0, false)]
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
