using FiscalCalendar.Api.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;
using System;
using System.Net;
using System.Threading.Tasks;
using FiscalCalendar.Services.Models;
using Microsoft.Extensions.Logging;

namespace FiscalCalendar.Api.Attributes
{
    /// <summary>
    /// There is an API Gateway level security by means of x-api-key in AWS.
    /// Further application/policy level security can be implemented in this class.
    /// For demo purposes, an Authorization header will need to be passed in, along with the x-api-key.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AuthorizationAttribute : Attribute, IAsyncActionFilter
    {
        private const string AuthorizationHeaderName = "Authorization";

        private readonly IFiscalCalendarSecrets _secrets;
        private readonly ILogger<AuthorizationAttribute> _logger;

        public AuthorizationAttribute(IFiscalCalendarSecrets secrets, ILogger<AuthorizationAttribute> logger)
        {
            _secrets = secrets;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var statusCode = HttpStatusCode.Unauthorized;
            var response = new MessageResponse { StatusCode = statusCode };

            if (!context.HttpContext.Request.Headers.TryGetValue(AuthorizationHeaderName, out var authorizationHeaderValue))
            {
                _logger.LogError("Authorization not provided in header.");
                response.Message = "'Authorization' key must be provided";
                context.Result = new ObjectResult(response) { StatusCode = (int)statusCode };
                return;
            }

            if (!authorizationHeaderValue.ToString().Equals(_secrets.AuthorizationKeySecret, StringComparison.CurrentCulture))
            {
                _logger.LogError("Authorization value is invalid.");
                response.Message = "Invalid 'Authorization' key";
                context.Result = new ObjectResult(response) { StatusCode = (int)statusCode };
                return;
            }

            _logger.LogInformation("Authorization successfully validated.");
            await next();
        }
    }
}
