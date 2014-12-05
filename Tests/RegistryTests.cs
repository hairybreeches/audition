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
        private string root;
        private readonly RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);
        private readonly Registry registry = new Registry(RegistryHive.CurrentUser);


        [Test]
        public void WhenWeTryGetValueAndTheLocationDoesntExistMethodReturnsFalse()
        {
            var location = GetLocationWhichDoesNotExist();
            string keyValue;
            var returnValue = registry.TryGetStringValue(location, "key", out keyValue);
            Assert.AreEqual(false, returnValue, "When the location does not exist we should not be able to read a value");
        }    
        
        [Test]        
        public void WhenWeTryGetValueAndTheLocationExistsButTheKeyDoesntMethodReturnsFalse()
        {
            var location = GetEmptyLocationWhichExists();
            string keyValue;
            var returnValue = registry.TryGetStringValue(location, "key", out keyValue);
            Assert.AreEqual(false, returnValue, "When the location exists but the key doesn't we should not be able to read a value");
        }        
        
        [Test]        
        public void WhenWeTryGetValueNamesAndTheLocationExistsWeGetTheEmptyList()
        {
            var location = GetEmptyLocationWhichExists();
            IEnumerable<string> valueNames;
            var returnValue = registry.TryGetValueNames(location, out valueNames);
            Assert.AreEqual(true, returnValue, "When the location exists we should get an empty list");
            CollectionAssert.IsEmpty(valueNames, "When the location exists we should get an empty list");
        }        

        [Test]
        public void WhenWeTryGetValueNamesAndTheLocationDoesntExistMethodReturnsFalse()
        {
            var location = GetLocationWhichDoesNotExist();
            IEnumerable<string> valueNames;
            var returnValue = registry.TryGetValueNames(location, out valueNames);
            Assert.AreEqual(false, returnValue, "When the location does not exist we should not be able to read value names");
        }

        private string GetLocationWhichDoesNotExist()
        {
            return root + "\\" + Guid.NewGuid();
        }    
        
        private string GetEmptyLocationWhichExists()
        {
            var location = GetLocationWhichDoesNotExist();
            CreateLocation(location);
            return location;
        }

        private void CreateLocation(string location)
        {
            baseKey.CreateSubKey(location);
        }

        [TestFixtureSetUp]
        public void SetUp()
        {            
            var fixtureKey = Guid.NewGuid().ToString();
            root = "Software\\Tests\\" + fixtureKey;
            CreateLocation(root);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            baseKey.DeleteSubKeyTree(root);
        }
    }
}
