using System;

namespace Native
{
    public static class RegistryExtensions
    {
        public static bool TryGetDateValue(this IRegistry registry, string location, string keyName, out DateTime keyValue)
        {
            string stringValue;
            if(!registry.TryGetStringValue(location, keyName, out stringValue))
            {
                keyValue = DateTime.MinValue;
                return false;
            }

            return DateTime.TryParse(stringValue, out keyValue);
        }

        public static DateTime EnsureValueExists(this ICurrentUserRegistry registry, string location, string keyName, DateTime defaultValue)
        {
            DateTime currentValue;
            if (registry.TryGetDateValue(location, keyName, out currentValue))
            {
                return currentValue;
            }

            registry.WriteValue(location, keyName, defaultValue);
            return defaultValue;
        }
    }
}