using System.Text.RegularExpressions;

namespace Sage50
{
    public class Sage50Driver
    {
        private Sage50Driver(int version, string name)
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
    }
}