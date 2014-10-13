using System;

namespace Model.SearchWindows
{
    public static class SearchWindowExtensions
    {
        public static DateTime CreationStartDate(this SearchWindow<YearEndParameters> searchWindow)
        {
            var periodEndDate = searchWindow.Period.To;
            return periodEndDate.Subtract(TimeSpan.FromDays(searchWindow.Parameters.DaysBeforeYearEnd));
        }
    }
}