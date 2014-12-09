namespace Licensing
{
    public interface ILicence
    {
        bool IsFullyLicensed { get; }
        int RemainingTrialDays { get; }
        bool TrialValid { get; }
    }
}
