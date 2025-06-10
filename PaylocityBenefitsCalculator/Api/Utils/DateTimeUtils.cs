namespace Api.Utils;

public static class DateTimeUtils
{
    public static int GetExactYearsDifference(DateTime startDate, DateTime endDate)
    {
        int years = endDate.Year - startDate.Year;

        // Check if the end date has not reached the full year
        if (endDate < startDate.AddYears(years))
        {
            years--;
        }

        return years;
    }
}