using Native;

namespace Licensing
{
    public class LicenceStorage
    {
        private readonly ICurrentUserRegistry registry;
        private readonly LicenceVerifier licenceVerifier;
        private const string LicenceKeyLocation = "SOFTWARE\\Audition\\Audition";
        private const string LicenceKeyName = "LicenceKey";


        public LicenceStorage(ICurrentUserRegistry registry, LicenceVerifier licenceVerifier)
        {
            this.registry = registry;
            this.licenceVerifier = licenceVerifier;
        }

        public ILicence GetLicence()
        {
            string licenceKey;
            return new Licence(registry.TryGetStringValue(LicenceKeyLocation, LicenceKeyName, out licenceKey));
        }

        public void StoreLicence(string licenceKey)
        {
            licenceVerifier.VerifyLicence(licenceKey);
            registry.WriteValue(LicenceKeyLocation, LicenceKeyName, licenceKey);
        }
    }
}
