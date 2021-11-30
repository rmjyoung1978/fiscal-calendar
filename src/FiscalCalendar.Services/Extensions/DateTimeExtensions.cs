using System;
using System.Collections.Generic;

namespace FiscalCalendar.Services.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime GetFirstDayOfCurrentWeek(this DateTime value, DayOfWeek firstDayOfWeek)
        {
            while (value.DayOfWeek != firstDayOfWeek) value = value.AddDays(-1);
            return value;
        }

        public static DateTime GetFirstDayOfFollowingWeek(this DateTime value, DayOfWeek firstDayOfWeek)
        {
            while (value.DayOfWeek != firstDayOfWeek) value = value.AddDays(1);
            return value;
        }

        public static double NumberOfDaysTillEndOfMonth(this DateTime value)
        {
            var monthEndDate = new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month));
            return value == monthEndDate ? 0 : (monthEndDate - value).TotalDays;
        }

        public static List<DateTime> GetAllDatesOfThisWeek(this DateTime value)
        {
            var endDate = value.AddWeeks(1);

            var dates = new List<DateTime>();

            for (var dt = value; dt < endDate; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }
            
            return dates;
        }

        public static DateTime AddWeeks(this DateTime date, int value)
        {
            return date.AddDays(7 * value);
        }
    }
}
