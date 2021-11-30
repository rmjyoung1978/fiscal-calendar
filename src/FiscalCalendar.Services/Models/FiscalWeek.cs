using System;
using System.Collections.Generic;

namespace FiscalCalendar.Services.Models
{
    public class FiscalWeek
    {
        /// <summary>
        /// Number of week within the year it falls in
        /// </summary>
        public int WeekNumber { get; set; }

        /// <summary>
        /// Dates covered in this week
        /// </summary>
        public List<string> Days { get; set; }
    }
}
