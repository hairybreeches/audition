using System;
using System.Collections.Generic;
using System.Linq;
using Native;
using NSubstitute;

namespace Tests.Mocks
{
    public class MockRegistryReader : IRegistryReader
    {        
        private readonly IDictionary<string, IEnumerable<string>> valueNamesLookup = new Dictionary<string, IEnumerable<string>>();
        private readonly Dictionary<Tuple<string, string>, string> valueLookup = new Dictionary<Tuple<string, string>, string>();


        public MockRegistryReader SetValueNames(string keyName, IEnumerable<string> valueNames)
        {
            this.valueNamesLookup[keyName] = valueNames;
            return this;
        } 
        
        public MockRegistryReader SetValue(string keyLocation, string keyName, string value)
        {
            valueLookup[new Tuple<string, string>(keyLocation, keyName)] = value;
            return this;
        }        

        public bool TryGetStringValue(string licenceKeyLocation, string licenceKeyName, out string licenceKey)
        {
            return valueLookup.TryGetValue(new Tuple<string, string>(licenceKeyLocation, licenceKeyName), out licenceKey);
        }

        public bool TryGetValueNames(string location, out IEnumerable<string> valueNames)
        {
            return valueNamesLookup.TryGetValue(location, out valueNames);
        }
    }
}