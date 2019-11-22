using System;

namespace TestPoint.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ToDateTime(
            this int value)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(value).ToLocalTime();
            return dtDateTime;
        }
    }
}
