using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ACVPCore.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Web.Admin.Auth
{
    public class PrincipalValidator
    {
        public static async Task ValidateAsync(CookieValidatePrincipalContext context)
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<PrincipalValidator>>();

            if (context == null)
            {
                logger.LogError($"{nameof(context)} was null");
                throw new ArgumentNullException(nameof(context));
            }
                
            // Find the email address in the claims
            var email = context.Principal.Claims.FirstOrDefault(f => f.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                logger.LogWarning($"no claims found for {nameof(ClaimTypes.Email)}");
                context.RejectPrincipal();
                return;
            }
                
            // ensure the email address is allowed to be authenticated.
            var adminUserService = context.HttpContext.RequestServices.GetRequiredService<IAdminUserService>();
            var isValid = await adminUserService.IsUserAuthorized(email);
            if (!isValid)
            {
                logger.LogWarning($"{email} is not authorized.");
                context.RejectPrincipal();
            }
        }
    }
}