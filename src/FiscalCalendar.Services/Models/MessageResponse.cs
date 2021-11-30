using System.Net;

namespace FiscalCalendar.Services.Models
{
    public class MessageResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public object Message { get; set; }
    }
}
