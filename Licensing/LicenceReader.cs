using Native;

namespace Licensing
{
    public class LicenceReader
    {
        private readonly IRegistryReader registryReader;
        private const string LicenceKeyLocation = "SOFTWARE\\Audition\\Audition\\Licence key";

        public LicenceReader(IRegistryReader registryReader)
        {
            this.registryReader = registryReader;
        }

        public ILicence GetLicence()
        {
            return new Licence(false);
        }
    }
}
