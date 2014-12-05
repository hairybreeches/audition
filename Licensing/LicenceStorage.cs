using Native;

namespace Licensing
{
    public class LicenceStorage
    {
        private readonly ICurrentUserRegistry registry;
        private const string LicenceKeyLocation = "SOFTWARE\\Audition\\Audition";
        private const string LicenceKeyName = "LicenceKey";


        public LicenceStorage(ICurrentUserRegistry registry)
        {
            this.registry = registry;
        }

        public ILicence GetLicence()
        {
            string licenceKey;
            return new Licence(registry.TryGetStringValue(LicenceKeyLocation, LicenceKeyName, out licenceKey));
        }

        public void StoreLicence(string licenceKey)
        {
            registry.WriteValue(LicenceKeyLocation, LicenceKeyName, licenceKey);
        }
    }
}
