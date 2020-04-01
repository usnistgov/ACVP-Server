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
            var error = new ErrorObject
            {
                Error = "Internal service error. Contact service provider."
            };
            
            Log.Error("EXCEPTION HANDLED");
            Log.Error(context.Exception.Message);
            Log.Error(context.Exception.StackTrace);
            
            context.ExceptionHandled = true;
            context.Result = new JsonHttpStatusResult(_jsonWriter.BuildVersionedObject(error), HttpStatusCode.InternalServerError);
        }
    }
}