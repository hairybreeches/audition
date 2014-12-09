using System;
using Native;

namespace Licensing
{
    public class LicenceStorage
    {
        private readonly ICurrentUserRegistry registry;
        private readonly LicenceVerifier licenceVerifier;
        private readonly IClock clock;
        private const string Location = "SOFTWARE\\Audition\\Audition";
        private const string LicenceKeyName = "LicenceKey";
        private const string TrialStartKeyName = "TrialStart";
        private int numberOfDaysInTrial = 28;


        public LicenceStorage(ICurrentUserRegistry registry, LicenceVerifier licenceVerifier, IClock clock)
        {
            this.registry = registry;
            this.licenceVerifier = licenceVerifier;
            this.clock = clock;
        }

        public ILicence GetLicence()
        {
            string licenceKey;
            var isFullyLicensed = registry.TryGetStringValue(Location, LicenceKeyName, out licenceKey);
            var expiryDate = registry.EnsureValueExists(Location, TrialStartKeyName, clock.GetCurrentDate());
            var daysOfTrialRemaining = (int) (expiryDate - clock.GetCurrentDate()).TotalDays;
            return new Licence(isFullyLicensed, daysOfTrialRemaining);
        }

        public void StoreLicence(string licenceKey)
        {
            licenceVerifier.VerifyLicence(licenceKey);
            registry.WriteValue(Location, LicenceKeyName, licenceKey);
        }
    }
}
