using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Web.Public.Results
{
    public class JsonHttpStatusResult : JsonResult
    {
        private readonly HttpStatusCode _statusCode;

        public JsonHttpStatusResult(object json, HttpStatusCode statusCode = HttpStatusCode.OK) : base(json)
        {
            _statusCode = statusCode;
        }

        public override void ExecuteResult(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)_statusCode;
            base.ExecuteResult(context);
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            context.HttpContext.Response.StatusCode = (int)_statusCode;
            return base.ExecuteResultAsync(context);
        }
    }
}