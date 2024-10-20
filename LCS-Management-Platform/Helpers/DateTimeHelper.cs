namespace LCS_Management_Platform.Helpers
{
    public static class DateTimeHelper
    {
        public static string CurrentTime()
        {
            var utcNow = DateTime.UtcNow;

            var britishTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

            var britishTime = TimeZoneInfo.ConvertTimeFromUtc(utcNow, britishTimeZone);

            return britishTime.ToString("dd/MM/yyyy HH:mm:ss");
        }

        public static string BritishTime(DateTime timeToChange)
        {
            var britishTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");

            var britishTime = TimeZoneInfo.ConvertTimeFromUtc(timeToChange, britishTimeZone);

            return britishTime.ToString("dd/MM/yyyy HH:mm:ss");
        }
    }
}
