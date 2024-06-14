using System;
using System.Globalization;

namespace SharedKernel.Extensions
{
    public static class DateUtilityExtension
    {
        public static string ToElasticDateFormat(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd'T'HH:mm:ss.fffK", CultureInfo.InvariantCulture);
        }

        public static int ToTotalMinutes(this DateTime date)
        {
            return (date.Hour * 60) + date.Minute;
        }
    }
}
