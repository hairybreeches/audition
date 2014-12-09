using System;

namespace Licensing
{
    class Licence : ILicence
    {
        public Licence(bool isFullyLicensed, int remainingTrialDays)
        {
            RemainingTrialDays = remainingTrialDays;
            IsFullyLicensed = isFullyLicensed;
        }

        public bool IsFullyLicensed { get; private set; }
        public int RemainingTrialDays { get; private set; }

        public bool TrialValid
        {
            get { return RemainingTrialDays >= 0; }
        }
    }
}