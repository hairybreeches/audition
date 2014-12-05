using Native;

namespace Licensing
{
    public class LicenceReader
    {
        private readonly ICurrentUserRegistryReader registryReader;
        private const string LicenceKeyLocation = "SOFTWARE\\Audition\\Audition";
        private const string LicenceKeyName = "LicenceKey";


        public LicenceReader(ICurrentUserRegistryReader registryReader)
        {
            this.registryReader = registryReader;
        }

        public ILicence GetLicence()
        {
            string licenceKey;
            return new Licence(registryReader.TryGetStringValue(LicenceKeyLocation, LicenceKeyName, out licenceKey));
        }
    }
}
