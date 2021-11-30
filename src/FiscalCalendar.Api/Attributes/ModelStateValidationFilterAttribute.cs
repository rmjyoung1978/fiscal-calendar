using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NLog;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using FiscalCalendar.Services.Models;
using Microsoft.Extensions.Logging;

namespace FiscalCalendar.Api.Attributes
{
    public class ModelStateValidationFilterAttribute : ActionFilterAttribute
    {
        private readonly ILogger<ModelStateValidationFilterAttribute> _logger;

        public ModelStateValidationFilterAttribute(ILogger<ModelStateValidationFilterAttribute> logger)
        {
            _logger = logger;
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;

            var messageResponses = new List<MessageResponse>();
            var modelStateEntries = context.ModelState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            foreach (var (key, value) in modelStateEntries)
            {
                if (value.ValidationState == ModelValidationState.Invalid)
                {
                    _logger.LogError($"Model state is invalid, property: {key}");
                }

                foreach (var error in value.Errors)
                {
                    var response = context.HttpContext.Response;
                    var errorMessage = CustomErrorMessages().FirstOrDefault(i => i.Key == key).Value;
                    if (string.IsNullOrWhiteSpace(errorMessage)) errorMessage = error.ErrorMessage;

                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.ContentType = "application/json";
                    messageResponses.Add(new MessageResponse { Message = errorMessage, StatusCode = HttpStatusCode.BadRequest });
                }
            }

            context.Result = new ObjectResult(messageResponses);
        }

        public static Dictionary<string, string> CustomErrorMessages()
        {
            return new Dictionary<string, string>
            {
                { "yearValue", "Invalid value for 'yearValue'. Please enter a number between 1 and 9998." },
                { "FirstDayOfWeek", "Invalid value for 'FirstDayOfWeek'. Please enter a number between 1 and 7 (e.g., 1 for Sunday, 2 for Monday and so on)." },
                { "FirstMonth", "Invalid value for 'FirstMonth'. Please enter a number between 1 and 12 (e.g., 1 for January, 2 for February and so on)." }
            };
        }
    }
}
