using System;

namespace Model.SearchWindows
{
    public class EndingParameters
    {
        public int MinimumZeroesToBeConsideredUnusual { get; private set; }

        public EndingParameters(int minimumZeroesToBeConsideredUnusual)
        {
            MinimumZeroesToBeConsideredUnusual = minimumZeroesToBeConsideredUnusual;
        }

        public int Magnitude()
        {
            return (int) Math.Pow(10, MinimumZeroesToBeConsideredUnusual);
        }

        public override string ToString()
        {
            return String.Format("Ending in at least {0} zeroes", MinimumZeroesToBeConsideredUnusual);
        }
    }
}