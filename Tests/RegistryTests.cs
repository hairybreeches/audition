using System;
using System.Collections.Generic;
using Microsoft.Win32;
using Native;
using NUnit.Framework;
using Registry = Native.Registry;

namespace Tests
{
    [TestFixture]
    public class RegistryTests
    {
        private string root;
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

        [Test]
        public void CanReadAKeyWhichExists()
        {
            var location = GetEmptyLocationWhichExists();
            WriteValue(location, "keyName", "value");
            string returnedValue;
            Assert.AreEqual(true, registry.TryGetStringValue(location, "keyName", out returnedValue));
            Assert.AreEqual("value", returnedValue, "The value read should be the one we wrote");
        }      
        
        
        [Test]
        public void CanWriteAKeyValueEvenIfTheLocationDoesNotExist()
        {
            var location = GetLocationWhichDoesNotExist();
            WriteValue(location, "keyName", "value");
            string returnedValue;
            Assert.AreEqual(true, registry.TryGetStringValue(location, "keyName", out returnedValue));
            Assert.AreEqual("value", returnedValue, "The value read should be the one we wrote");
        }  
        
        [Test]
        public void CanReadAListOfKeyValuesWhichExist()
        {
            var location = GetEmptyLocationWhichExists();
            WriteValue(location, "keyName", "value");
            WriteValue(location, "keyName2", "value");
            WriteValue(location, "another key name", "value");
            IEnumerable<string> valueNames;
            Assert.AreEqual(true, registry.TryGetValueNames(location, out valueNames));
            CollectionAssert.AreEquivalent(new[]{"keyName", "keyName2", "another key name"}, valueNames, "The value names read should be the ones we wrote");
        }

        [Test]
        public void IfValueIsNotDateTryGetDateValueReturnsFalse()
        {
            var location = GetEmptyLocationWhichExists();
            WriteValue(location, "not a date", "steve");
            DateTime value;
            Assert.AreEqual(false, registry.TryGetDateValue(location, "not a date", out value), "if the value can't be parsed as a date, we shouldn't get anything back");
        }
        
        [Test]
        public void IfValueIsADateTryGetWeGetItBack()
        {
            var location = GetEmptyLocationWhichExists();
            var dateTime = new DateTime(1999,12,31);
            WriteValue(location, "date", dateTime);
            DateTime value;
            Assert.AreEqual(true, registry.TryGetDateValue(location, "date", out value), "if the value can be parsed as a date, we should get it back");
            Assert.AreEqual(dateTime, value, "if the value can be parsed as a date, we should get it back");
        }

        private void WriteValue(string location, string name, object value)
        {
            registry.WriteValue(location, name, value);
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
            registry.CreateLocation(location);
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
            registry.DeleteLocation(root);
            registry.Dispose();
        }
    }
}
