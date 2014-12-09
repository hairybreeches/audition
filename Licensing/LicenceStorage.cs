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
        private const int NumberOfDaysInTrial = 28;


        public LicenceStorage(ICurrentUserRegistry registry, LicenceVerifier licenceVerifier, IClock clock)
        {
            this.registry = registry;
            this.licenceVerifier = licenceVerifier;
            this.clock = clock;
        }

        public ILicence GetLicence()
        {
            var isFullyLicensed = IsFullyLicensed();
            var daysOfTrialRemaining = DaysOfTrialRemaining();
            return new Licence(isFullyLicensed, daysOfTrialRemaining);
        }

        private bool IsFullyLicensed()
        {
            string licenceKey;
            return registry.TryGetStringValue(Location, LicenceKeyName, out licenceKey);
        }

        private int DaysOfTrialRemaining()
        {
            var trialStartDate = registry.EnsureValueExists(Location, TrialStartKeyName, clock.GetCurrentDate());
            var expiryDate = trialStartDate.AddDays(NumberOfDaysInTrial);
            return (int) (expiryDate - clock.GetCurrentDate()).TotalDays;
        }

        public void StoreLicence(string licenceKey)
        {
            licenceVerifier.VerifyLicence(licenceKey);
            registry.WriteValue(Location, LicenceKeyName, licenceKey);
        }
    }
}
