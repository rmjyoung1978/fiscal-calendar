using System.Collections.Generic;

namespace FiscalCalendar.Services.Models
{
    public class FiscalYear
    {
        public int Year { get; set; }

        /// <summary>
        /// Year divided into 4 quarters of 13 weeks each
        /// The last quarter could contain 14 weeks
        /// </summary>
        public List<FiscalQuarter> Quarters { get; set; }

        public FiscalYear()
        {
            Quarters = new List<FiscalQuarter>();

            // There will be 4 quarters always
            for (var i = 1; i <= 4; i++)
            {
                Quarters.Add(new FiscalQuarter { QuarterNumber = i});
            }

            foreach (var quarter in Quarters)
            {
                quarter.FiscalPeriods = new List<FiscalPeriodInQuarter>();

                // There will be 3 periods per quarter (4-4-5 or 4-4-6)
                for (var i = 1; i <= 3; i++)
                {
                    quarter.FiscalPeriods.Add(new FiscalPeriodInQuarter { PeriodNumber = i, Weeks = new List<FiscalWeek>()});
                }
            }
        }
    }
}
