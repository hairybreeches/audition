namespace Model.SearchWindows
{
    public class YearEndParameters
    {
        public YearEndParameters(int daysBeforeYearEnd)
        {
            DaysBeforeYearEnd = daysBeforeYearEnd;
        }

        public int DaysBeforeYearEnd { get; private set; }
    }
}