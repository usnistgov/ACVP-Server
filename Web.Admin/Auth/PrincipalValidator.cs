using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using NIST.CVP.Libraries.Internal.ACVPCore.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Web.Admin.Auth
{
    public class PrincipalValidator
    {
        public static async Task ValidateAsync(SecurityTokenValidatedContext context)
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
                var noEmailClaims = $"No claims found for {nameof(ClaimTypes.Email)}."; 
                logger.LogWarning(noEmailClaims);
                context.Fail(noEmailClaims);
                return;
            }
                
            // ensure the email address is allowed to be authenticated.
            var adminUserService = context.HttpContext.RequestServices.GetRequiredService<IAdminUserService>();
            var isValid = await adminUserService.IsUserAuthorized(email);
            if (!isValid)
            {
                var unauthorizedUserMessage = $"{email} is not authorized."; 
                logger.LogWarning(unauthorizedUserMessage);
                context.Fail(unauthorizedUserMessage);
            }
        }
    }
}