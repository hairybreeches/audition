using System;
using System.Text.RegularExpressions;

namespace Sage50
{
    public class Sage50Driver
    {
        public Sage50Driver(int version, string name)
        {
            Version = version;
            Name = name;
        }

        public int Version { get; private set; }
        public string Name { get; private set; }

        public static Sage50Driver Create(string name)
        {
            var version = ParseVersion(name);
            return new Sage50Driver(version, name);
        }

        private static int ParseVersion(string name)
        {
            var regex = new Regex("\\d+$");           
            var versionString = regex.Match(name).Value;
            return int.Parse(versionString);
        }


        public override string ToString()
        {
            return String.Format("{0} (Version {1})", Name, Version);
        }

        protected bool Equals(Sage50Driver other)
        {
            return Version == other.Version && string.Equals(Name, other.Name);
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