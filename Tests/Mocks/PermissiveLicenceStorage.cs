using Licensing;

namespace Tests.Mocks
{
    public class PermissiveLicenceStorage : ILicenceStorage
    {
        public Licence GetLicence()
        {
            return new Licence(true, int.MaxValue);
        }

        public void StoreLicence(string licenceKey)
        {            
        }

        public void EnsureUseAllowed()
        {         
        }
    }
}
