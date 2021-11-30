using FiscalCalendar.Services.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;

namespace FiscalCalendar.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected ActionResult FlowResult(ValueTuple<object, HttpStatusCode> result)
        {
            var (o, statusCode) = result;
            var oString = o?.ToString();

            string message;

            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    return Ok(o);

                default:
                    message = oString;
                    break;
            }

            var actionResult = new ObjectResult(new MessageResponse { StatusCode = statusCode, Message = message })
            {
                StatusCode = (int)statusCode
            };

            return actionResult;
        }
    }
}
