namespace Licensing
{
    class Licence : ILicence
    {
        public Licence(bool isFullyLicensed)
        {
            IsFullyLicensed = isFullyLicensed;
        }

        public bool IsFullyLicensed { get; private set; }
    }
}