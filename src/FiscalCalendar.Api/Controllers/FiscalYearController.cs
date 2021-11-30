using FiscalCalendar.Api.Attributes;
using FiscalCalendar.Services;
using FiscalCalendar.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using Microsoft.Extensions.Logging;
using ILogger = NLog.ILogger;

namespace FiscalCalendar.Api.Controllers
{
    [TypeFilter(typeof(AuthorizationAttribute))]
    [ApiController]
    [ExcludeFromCodeCoverage]
    [Route("[controller]")]
    public class FiscalYearController : BaseController
    {
        private readonly ICalendarService _calendarService;
        private readonly ILogger<FiscalYearController> _logger;

        public FiscalYearController(ICalendarService calendarService, ILogger<FiscalYearController> logger)
        {
            _calendarService = calendarService;
            _logger = logger;
        }

        // GET: fiscalyear/2020
        [HttpGet("{yearValue}")]
        public ActionResult Get(CancellationToken cancellationToken, [Range(1, 9998)] int yearValue, [FromQuery] CalendarOptions options)
        {
            return FlowResult(_calendarService.GetYear(cancellationToken, yearValue, options));
        }
    }
}
