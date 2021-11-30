using System.Net;
using System.Threading;
using FiscalCalendar.Services.Models;

namespace FiscalCalendar.Services
{
    public interface ICalendarService
    {
        (object, HttpStatusCode) GetYear(CancellationToken cancellationToken, int yearValue, CalendarOptions options);
    }
}
