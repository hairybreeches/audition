using Native;
using Native.RegistryAccess;
using Native.Time;

namespace Licensing
{
    public class LicenceStorage : ILicenceStorage
    {
        private readonly ICurrentUserRegistry registry;
        private readonly LicenceVerifier licenceVerifier;
        private readonly IClock clock;
        private const string Location = "SOFTWARE\\Audition\\Audition";
        private const string LicenceKeyName = "LicenceKey";
        private const string TrialStartKeyName = "TrialStart";
        private const int NumberOfDaysInTrial = 14;


        public LicenceStorage(ICurrentUserRegistry registry, LicenceVerifier licenceVerifier, IClock clock)
        {
            this.registry = registry;
            this.licenceVerifier = licenceVerifier;
            this.clock = clock;
        }

        public Licence GetLicence()
        {
            var isFullyLicensed = IsFullyLicensed();
            var daysOfTrialRemaining = DaysOfTrialRemaining();
            return new Licence(isFullyLicensed, daysOfTrialRemaining);
        }

        private bool IsFullyLicensed()
        {
            string licenceKey;
            if (!registry.TryGetStringValue(Location, LicenceKeyName, out licenceKey))
            {
                return false;
            }

            return licenceVerifier.IsValid(licenceKey);
        }

        private int DaysOfTrialRemaining()
        {
            var trialStartDate = registry.EnsureValueExists(Location, TrialStartKeyName, clock.GetCurrentDate());
            var expiryDate = trialStartDate.AddDays(NumberOfDaysInTrial);
            return (int) (expiryDate - clock.GetCurrentDate()).TotalDays;
        }

        public void StoreLicence(string licenceKey)
        {
            var cleanedKey = licenceKey.Trim().ToUpper();
            licenceVerifier.VerifyLicence(cleanedKey);
            registry.WriteValue(Location, LicenceKeyName, cleanedKey);
        }

        public void EnsureUseAllowed()
        {
            var licence = GetLicence();
            if (!licence.AllowUse)
            {
                throw new UnlicensedException();
            }
        }
    }
}
