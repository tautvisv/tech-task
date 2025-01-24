namespace Claims.Utils
{
    public class UtcDateTimeService : IDateTimeService
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.UtcNow;
        }
    }
}
