﻿using System.Collections.Generic;
using Native;
using NSubstitute;

namespace Tests.Mocks
{
    public class MockRegistryReader : IRegistryReader
    {
        private readonly IDictionary<string, IRegistryKey> lookup = new Dictionary<string, IRegistryKey>();

        public bool TryOpenKey(string keyName, out IRegistryKey registryKey)
        {
            return lookup.TryGetValue(keyName, out registryKey);
        }

        public MockRegistryReader SetValueNames(string keyName, IDictionary<string, string> values)
        {
            lookup[keyName] = new MockRegistryKey(values);
            return this;
        }

        private class MockRegistryKey : IRegistryKey
        {
            private readonly IDictionary<string, string> values;

            public MockRegistryKey(IDictionary<string, string> values)
            {
                this.values = values;
            }

            public IEnumerable<string> GetValueNames()
            {
                return values.Keys;
            }

            public bool TryGetStringValue(string name, out string value)
            {
                return values.TryGetValue(name, out value);
            }
        }

    }
}