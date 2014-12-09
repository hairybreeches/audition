using System;

namespace Licensing
{
    public class Licence
    {
        private readonly int remainingTrialDays;

        public Licence(bool isFullyLicensed, int remainingTrialDays)
        {
            this.remainingTrialDays = remainingTrialDays;
            IsFullyLicensed = isFullyLicensed;
        }

        public bool IsFullyLicensed { get; private set; }

        public int RemainingTrialDays
        {
            get { return Math.Max(remainingTrialDays, 0); }
        }

        public bool TrialValid
        {
            get { return remainingTrialDays >= 0; }
        }

        public bool AllowUse
        {
            get { return IsFullyLicensed || TrialValid; }
        }
    }
}