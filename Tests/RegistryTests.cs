using System;
using System.Collections.Generic;
using Microsoft.Win32;
using NUnit.Framework;
using Registry = Native.Registry;

namespace Tests
{
    [TestFixture]
    public class RegistryTests
    {
        private string fixtureKey;
        private RegistryKey testKey;

        private string Root
        {
            get { return "Software\\Tests\\" + fixtureKey; }
        }

        [Test]
        public void WhenWeTryGetValueAndTheLocationDoesntExistMethodReturnsFalse()
        {
            var location = GetLocationWhichDoesNotExist();
            string keyValue;
            var returnValue = new Registry(RegistryHive.CurrentUser).TryGetStringValue(location, "key", out keyValue);
            Assert.AreEqual(false, returnValue, "When the location does not exist we should not be able to read a value");
        }        

        [Test]
        public void WhenWeTryGetValueNamesAndTheLocationDoesntExistMethodReturnsFalse()
        {
            var location = GetLocationWhichDoesNotExist();
            IEnumerable<string> valueNames;
            var returnValue = new Registry(RegistryHive.CurrentUser).TryGetValueNames(location, out valueNames);
            Assert.AreEqual(false, returnValue, "When the location does not exist we should not be able to read value names");
        }

        private string GetLocationWhichDoesNotExist()
        {
            return Root + "\\" + Guid.NewGuid();
        }

        [TestFixtureSetUp]
        public void SetUp()
        {
            var softwareKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32).OpenSubKey("Software", RegistryKeyPermissionCheck.ReadWriteSubTree);
            testKey = softwareKey.CreateSubKey("Tests");
            fixtureKey = Guid.NewGuid().ToString();
            testKey.CreateSubKey(fixtureKey);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            testKey.DeleteSubKey(fixtureKey);
        }
    }
}
