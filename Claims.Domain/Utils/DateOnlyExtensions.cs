namespace Claims.Domain.Utils
{
    public static class DateOnlyExtensions
    {
        public static bool IsDateInRange(this DateOnly currentDate, DateOnly startDate, DateOnly endDate)
        {
            return currentDate >= startDate && startDate <= endDate;
        }
    }
}
