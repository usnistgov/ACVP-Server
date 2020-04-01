using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using Web.Public.JsonObjects;
using Web.Public.Results;
using Web.Public.Services;

namespace Web.Public.Exceptions
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly IJsonWriterService _jsonWriter;
        
        public ExceptionFilter(IJsonWriterService jsonWriter)
        {
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

            Log.Error(context.Exception, "EXCEPTION HANDLED");
            
            context.ExceptionHandled = true;
            context.Result = new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(error), HttpStatusCode.InternalServerError);
        }

        private ErrorObject OnJsonReaderException(JsonReaderException ex)
        {
            return new ErrorObject
            {
                Error = ex.Message
            };
        }
    }
}