using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Web.Public.JsonObjects;
using Web.Public.Results;
using Web.Public.Services;

namespace Web.Public.Exceptions
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ExceptionFilter> _logger;
        private readonly IJsonWriterService _jsonWriter;
        
        public ExceptionFilter(ILogger<ExceptionFilter> logger, IJsonWriterService jsonWriter)
        {
            _logger = logger;
            _jsonWriter = jsonWriter;
        }
        
        public void OnException(ExceptionContext context)
        {
            ErrorObject error;
            HttpStatusCode statusCode;
            bool logAsError = false;

            switch (context.Exception)
            {
                // thrown by System.Json
                case JsonException e:
                    error = new ErrorObject()
                    {
                        Error = "Invalid JSON provided.",
                        Context = e.Message
                    };
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                // Thrown by our code, can contain multiple parameter parsing validation error messages
                case JsonReaderException e:
                    error = new ErrorObject()
                    {
                        Error = "Invalid JSON provided.",
                        Context = e.Errors
                    };
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                case PayloadValidatorException e:
                    error = new ErrorObject()
                    {
                        Error = "Validation error(s) on JSON payload.",
                        Context = e.Errors
                    };
                    statusCode = HttpStatusCode.BadRequest;
                    break;
                default:
                    error = new ErrorObject()
                    {
                        Error = "Internal service error. Contact service provider."
                    };
                    statusCode = HttpStatusCode.InternalServerError;
                    logAsError = true;
                    
                    break;
            }

            if (logAsError)
            {
                _logger.LogError(context.Exception, $"Exception Handled.");
            }
            
            context.ExceptionHandled = true;
            context.Result = new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(error), statusCode);
        }
    }
}