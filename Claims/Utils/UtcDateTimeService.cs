namespace Claims.Utils
{
    public class UtcDateTimeService : IDateTimeService
    {
        public DateTime GetCurrentDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}
