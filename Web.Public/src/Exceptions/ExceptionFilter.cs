using System.Net;
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
            var error = context.Exception switch
            {
                JsonReaderException ex => OnJsonReaderException(ex),
                _ => new ErrorObject
                {
                    Error = "Internal service error. Contact service provider."
                }
            };

            _logger.LogError(context.Exception, $"Exception Handled. {context.Exception.Message}");
            
            context.ExceptionHandled = true;
            context.Result = new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(error), HttpStatusCode.InternalServerError);
        }

        private ErrorObject OnJsonReaderException(JsonReaderException ex)
        {
            return new ErrorObject
            {
                Error = "Invalid JSON provided",
                Context = ex.Errors
            };
        }
    }
}