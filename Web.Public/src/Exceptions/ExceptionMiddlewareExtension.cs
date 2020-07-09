using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Web.Public.JsonObjects;
using Web.Public.Services;

namespace Web.Public.Exceptions
{
	public static class ExceptionMiddlewareExtension
	{
		public static void ConfigureExceptionMiddleware(this IApplicationBuilder app, ILogger logger, IJsonWriterService jsonWriter)
		{
			app.UseExceptionHandler(appError =>
			{
				appError.Run(async httpContext =>
				{
					var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();
					if (contextFeature != null)
					{
						logger.LogDebug("Handling exception within middleware.");
						
						ErrorObject error;
						HttpStatusCode statusCode;

						switch (contextFeature.Error)
						{
							case SecurityTokenExpiredException e:
								error = new ErrorObject()
								{
									Error = $"The provided JWT expired at {e.Expires:u} and is no longer valid. The JWT claims may be refreshed through the renewal process described in the specification."
								};
								statusCode = HttpStatusCode.Unauthorized;
								break;
							case SecurityTokenInvalidSignatureException e:
								error = new ErrorObject()
								{
									Error = "The provided JWT did not verify."
								};
								statusCode = HttpStatusCode.Forbidden;
								logger.LogError(contextFeature.Error, "JWT did not verify.");
								break;
							default:
								error = new ErrorObject()
								{
									Error = "Internal service error. Contact service provider."
								};
								statusCode = HttpStatusCode.InternalServerError;
								logger.LogError(contextFeature.Error, "Generic exception handled.");
								break;
						}

						httpContext.Response.ContentType = "application/json";
						httpContext.Response.StatusCode = (int) statusCode;

						await httpContext.Response.WriteAsync(
							JsonSerializer.Serialize(jsonWriter.BuildVersionedObject(error)));
					}
				});
			});
		}
	}
}