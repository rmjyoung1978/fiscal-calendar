using System.Collections.Generic;
using Newtonsoft.Json;

namespace FiscalCalendar.Services.Models
{
    public class FiscalPeriodInQuarter
    {
        [JsonIgnore]
        public int PeriodNumber { get; set; }

        /// <summary>
        /// The month with the date of 1st falling in
        /// </summary>
        public string FiscalMonth { get; set; }

        /// <summary>
        /// This would 4/5/6 weeks
        /// </summary>
        public int NumberOfFiscalWeeks { get; set; }

        /// <summary>
        /// Weeks in this period of the quarter
        /// </summary>
        public List<FiscalWeek> Weeks { get; set; }
    }
}
