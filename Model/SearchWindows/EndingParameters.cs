namespace Model.SearchWindows
{
    public class EndingParameters
    {
        public int MinimumZeroesToBeConsideredUnusual { get; private set; }

        public EndingParameters(int minimumZeroesToBeConsideredUnusual)
        {
            MinimumZeroesToBeConsideredUnusual = minimumZeroesToBeConsideredUnusual;
        }
    }
}