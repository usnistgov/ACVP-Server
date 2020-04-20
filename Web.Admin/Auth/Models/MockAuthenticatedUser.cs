using System;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Dasync.Collections;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Enums;

namespace Web.Admin.Auth.Models
{
    public class MockAuthenticatedUser : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string NameIdentifier = "myLegitUserId";
        public const string Name = "Test User";
        public const string Role = "Admin";
        public const string Email = "testUser@nist.gov";
        
        private readonly SsoConfig _ssoConfig;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly string[] _validEnvironmentsForMockAuthentication =
        {
            Environments.Local.ToString(),
            Environments.Dev.ToString(),
            Environments.Tc.ToString()
        };

        public MockAuthenticatedUser(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock, IOptions<SsoConfig> ssoConfig, IWebHostEnvironment webHostEnvironment)
            : base(options, logger, encoder, clock)
        {
            _ssoConfig = ssoConfig.Value;
            _webHostEnvironment = webHostEnvironment;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (_ssoConfig.UseSso)
            {
                throw new ArgumentException($"{nameof(_ssoConfig.UseSso)} should not be true when working with a mock authentication handler.");
            }

            // TODO uncomment this once SSO is figured out
            // if (!_validEnvironmentsForMockAuthentication.Contains(_webHostEnvironment.EnvironmentName, StringComparer.OrdinalIgnoreCase))
            // {
            //     throw new InvalidOperationException($"Attempted to use mock authentication in an environment ({_webHostEnvironment.EnvironmentName}) that is not allowed.");
            // }
            
            var claims = new[] 
            {
                new Claim(ClaimTypes.NameIdentifier, NameIdentifier),
                new Claim(ClaimTypes.Name, Name),
                new Claim(ClaimTypes.Role, Role),
                new Claim(ClaimTypes.Email, Email),
                new Claim(ClaimTypes.AuthenticationMethod, Scheme.Name), 
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return await Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}