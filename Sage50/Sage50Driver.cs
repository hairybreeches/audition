using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Sage50
{
    public class Sage50Driver
    {
        public Sage50Driver(int version, string name, string friendlyName)
        {
            FriendlyName = friendlyName;
            Version = version;
            Name = name;
        }

        public int Version { get; private set; }
        public string Name { get; private set; }
        public string FriendlyName { get; private set; }

        public static Sage50Driver Create(string name)
        {
            var version = ParseVersion(name);
            return new Sage50Driver(version, name, GetFriendlyName(name));
        }

        public String GetDemoDataLocation()
        {
            return "%PROGRAMDATA%\\Sage\\Accounts\\" + FriendlyName + "\\DEMODATA";
        }

        private static readonly IDictionary<string, string> knownFriendlyNames = new Dictionary<string, string>
        {
            {"Sage Line 50 v21", "2015"},
            {"Sage Line 50 v20", "2014"},
            {"Sage Line 50 v19", "2013"},
            {"Sage Line 50 v18", "2012"},
            {"Sage Line 50 v17", "2011"},
            {"Sage Line 50 v16", "2010"},
        };

        private static string GetFriendlyName(string name)
        {
            string friendlyName;

            return knownFriendlyNames.TryGetValue(name, out friendlyName) ? friendlyName : name;
        }

        private static int ParseVersion(string name)
        {
            var regex = new Regex("\\d+$");           
            var versionString = regex.Match(name).Value;
            return int.Parse(versionString);
        }


        public override string ToString()
        {
            return String.Format("{0} (Version {1}) - {2}", Name, Version, FriendlyName);
        }

        protected bool Equals(Sage50Driver other)
        {
            return Version == other.Version && string.Equals(Name, other.Name) && string.Equals(FriendlyName, other.FriendlyName);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Sage50Driver) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Version*397) ^ (Name != null ? Name.GetHashCode() : 0);
            }
        }
    }
}