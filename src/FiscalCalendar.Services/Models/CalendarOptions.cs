using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FiscalCalendar.Services.Models
{
    public class CalendarOptions
    {
        [FromQuery]
        [Range(1, 7)]
        public int FirstDayOfWeek { get; set; } = 1;

        [FromQuery]
        [Range(1, 12)]
        public int FirstMonth { get; set; } = 4;
    }
}
