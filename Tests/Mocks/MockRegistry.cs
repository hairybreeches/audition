using System;
using System.Collections.Generic;
using Native;

namespace Tests.Mocks
{
    public class MockRegistry : ICurrentUserRegistry, ILocalMachineRegistry
    {        
        private readonly IDictionary<string, IEnumerable<string>> valueNamesLookup = new Dictionary<string, IEnumerable<string>>();
        private readonly Dictionary<Tuple<string, string>, string> valueLookup = new Dictionary<Tuple<string, string>, string>();

        public bool TryGetStringValue(string location, string keyName, out string keyValue)
        {
            return valueLookup.TryGetValue(new Tuple<string, string>(location, keyName), out keyValue);
        }

        public bool TryGetValueNames(string location, out IEnumerable<string> valueNames)
        {
            return valueNamesLookup.TryGetValue(location, out valueNames);
        }

        public void WriteValue(string location, string name, object value)
        {
            SetValue(location, name, value.ToString());
        }

        public MockRegistry SetValueNames(string keyName, IEnumerable<string> valueNames)
        {
            valueNamesLookup[keyName] = valueNames;
            return this;
        } 
        
        public MockRegistry SetValue(string keyLocation, string keyName, string value)
        {
            valueLookup[new Tuple<string, string>(keyLocation, keyName)] = value;
            return this;
        }                

        public static ILocalMachineRegistry CreateRegistryWithSage50Drivers(params string[] drivers)
        {
            return new MockRegistry()
                .SetValueNames("SOFTWARE\\ODBC\\ODBCINST.INI\\ODBC Drivers", drivers);
        }

        public MockRegistry SetLicenceKey(string licenceKey)
        {
            SetValue("SOFTWARE\\Audition\\Audition", "LicenceKey", licenceKey);
            return this;
        }
    }
}