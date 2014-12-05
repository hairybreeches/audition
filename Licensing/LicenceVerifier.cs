namespace Licensing
{
    public class LicenceVerifier
    {
        public void VerifyLicence(string licenceKey)
        {
            if (licenceKey.Length != 16)
            {
                throw new InvalidLicenceKeyException();
            }
        }
    }
}