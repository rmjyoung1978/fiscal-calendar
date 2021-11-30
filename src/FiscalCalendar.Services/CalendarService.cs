using FiscalCalendar.Services.Extensions;
using FiscalCalendar.Services.Models;
using NLog;
using System;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ILogger = NLog.ILogger;

namespace FiscalCalendar.Services
{
    public class CalendarService : ICalendarService
    {
        public DayOfWeek FirstDayOfWeek { get; set; }

        public int FirstFiscalMonth { get; set; }
        
        private const int StandardNumberOfWeeks = 52;
        private static readonly int[] PeriodIncrementor = { 4, 8, 17, 21, 30, 34, 43, 47 };
        private static readonly int[] QuarterIncrementor = { 13, 26, 39 };
        private readonly ILogger<CalendarService> _logger;

        public CalendarService(ILogger<CalendarService> logger)
        {
            _logger = logger;
        }

        public (object, HttpStatusCode) GetYear(CancellationToken cancellationToken, int yearValue, CalendarOptions options)
        {
            FirstDayOfWeek = (DayOfWeek)options.FirstDayOfWeek-1;
            FirstFiscalMonth = options.FirstMonth;

            // Check the validity of the yearValue provided
            if (!DateTime.TryParse($"1/1/{yearValue}", out _))
            {
                _logger.LogError($"Invalid value '{yearValue}' provided for year.");
                return ("Invalid year provided.", HttpStatusCode.BadRequest);
            }

            var firstOfTheFiscalMonth = new DateTime(yearValue, FirstFiscalMonth, 1);
            var finalStartDate = GetStartDate(firstOfTheFiscalMonth);
            _logger.LogInformation($"First day of year calculated as {finalStartDate}");

            var finalEndDate = GetEndDate(finalStartDate);
            _logger.LogInformation($"Last day of year calculated as {finalEndDate}");


            var fiscalYear = GetFiscalYear(yearValue, finalStartDate, finalEndDate);
            _logger.LogInformation($"Fiscal year calculated as - { JsonConvert.SerializeObject(fiscalYear) }");

            return (fiscalYear, HttpStatusCode.OK);
        }

        private DateTime GetEndDate(DateTime finalStartDate)
        {
            // Add 52 weeks - remove one day so that the final end date is 'inclusive'
            var finalEndDate = finalStartDate.AddWeeks(StandardNumberOfWeeks).AddDays(-1);

            // If there are 4 or more days till the end of the month in which the finalEndDate falls
            // Then add on another week, to count it in this year as a 53rd week
            var numberOfDaysTillEndOfMonth = finalEndDate.NumberOfDaysTillEndOfMonth();
            if (numberOfDaysTillEndOfMonth >= 4 && numberOfDaysTillEndOfMonth < 7) finalEndDate = finalEndDate.AddWeeks(1);
            return finalEndDate;
        }

        private DateTime GetStartDate(DateTime firstOfTheFiscalMonth)
        {
            var firstDayOfThisWeek = new DateTime();

            if (firstOfTheFiscalMonth.DayOfWeek != FirstDayOfWeek)
                firstDayOfThisWeek = firstOfTheFiscalMonth.GetFirstDayOfCurrentWeek(FirstDayOfWeek);

            // If there were 4 or more days to get to the FirstDayOfWeek, it means this current week will have been counted as a 53rd week in/of the previous year
            // In the above scenario, the first day of the 'following week' will become the final start date of this year
            // If less than 4 days, then count this week in this year - and the first day of this current week will be the final start date of this year
            var finalStartDate = (firstOfTheFiscalMonth - firstDayOfThisWeek).TotalDays >= 4 ? firstOfTheFiscalMonth.GetFirstDayOfFollowingWeek(FirstDayOfWeek) : firstDayOfThisWeek;
            return finalStartDate;
        }

        private FiscalYear GetFiscalYear(int yearValue, DateTime startDate, DateTime endDate)
        {
            var fiscalYear = new FiscalYear { Year = yearValue };

            var standardNumberOfWeeks = StandardNumberOfWeeks;
            var firstMonth = FirstFiscalMonth;
            var counterDate = startDate;
            var quarter = 1;
            var period = 1;

            for (var i = 1; i <= standardNumberOfWeeks; i++)
            {
                var allDaysThisWeek = counterDate.GetAllDatesOfThisWeek();
                var weekEndDate = allDaysThisWeek.LastOrDefault();

                fiscalYear
                    .Quarters.FirstOrDefault(q => q.QuarterNumber == quarter)?
                    .FiscalPeriods.FirstOrDefault(p => p.PeriodNumber == period)?
                    .Weeks.Add(new FiscalWeek { WeekNumber = i, Days = allDaysThisWeek.Select(a => a.ToString("dd/MM/yyyy")).ToList() });

                // If this is the last round, but the calculated endDate has not been reached then make an extra round for 53rd week
                // This will be added to the last quarter's last period as a 6th week
                if (weekEndDate < endDate) standardNumberOfWeeks = standardNumberOfWeeks + 1;

                if (PeriodIncrementor.Contains(i)) period++;
                if (QuarterIncrementor.Contains(i)) { quarter++; period = 1; }

                // Increment counterDate to get the next date after the previous 7 days have been catered for
                counterDate = weekEndDate.AddDays(1);
            }

            foreach (var p in fiscalYear.Quarters.SelectMany(q => q.FiscalPeriods))
            {
                p.NumberOfFiscalWeeks = p.Weeks.Count;
                p.FiscalMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(firstMonth);
                firstMonth = firstMonth == 12 ? 1 : firstMonth + 1;
            }

            return fiscalYear;
        }
    }
}
