using Native;

namespace Licensing
{
    public class LicenceReader
    {
        private readonly ICurrentUserRegistry registry;
        private const string LicenceKeyLocation = "SOFTWARE\\Audition\\Audition";
        private const string LicenceKeyName = "LicenceKey";


        public LicenceReader(ICurrentUserRegistry registry)
        {
            this.registry = registry;
        }

        public ILicence GetLicence()
        {
            string licenceKey;
            return new Licence(registry.TryGetStringValue(LicenceKeyLocation, LicenceKeyName, out licenceKey));
        }
    }
}
