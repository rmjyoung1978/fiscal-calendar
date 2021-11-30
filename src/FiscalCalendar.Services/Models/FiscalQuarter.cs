using System.Collections.Generic;

namespace FiscalCalendar.Services.Models
{
    public class FiscalQuarter
    {
        public int QuarterNumber { get; set; }

        /// <summary>
        /// Q1, Q2 etc.
        /// </summary>
        public string QuarterName => $"Q{QuarterNumber}";


        /// <summary>
        /// Going by 4-4-5 calendar, there should only ever be 3 periods
        /// First and second periods in a quarter with 4 weeks
        /// Third period in a quarter with either 5 or 6 weeks (determined by the year and the calendar type selected)
        /// </summary>
        public List<FiscalPeriodInQuarter> FiscalPeriods { get; set; }
    }
}
