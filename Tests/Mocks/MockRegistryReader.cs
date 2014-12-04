using System.Collections.Generic;
using Native;
using NSubstitute;

namespace Tests.Mocks
{
    public class MockRegistryReader : IRegistryReader
    {
        private readonly IDictionary<string, IRegistryKey> lookup = new Dictionary<string, IRegistryKey>();

        public IRegistryKey OpenKey(string keyName)
        {
            return lookup[keyName];
        }

        public bool TryOpenKey(string keyName, out IRegistryKey registryKey)
        {
            return lookup.TryGetValue(keyName, out registryKey);
        }

        public MockRegistryReader SetValueNames(string keyName, IEnumerable<string> values)
        {
            var key = Substitute.For<IRegistryKey>();
            key.GetValueNames().Returns(values);
            lookup[keyName] = key;
            return this;
        }
    }
}