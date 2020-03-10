using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Web.Admin.Auth.Models
{
    public class MockAuthenticatedUser : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string NameIdentifier = "myLegitUserId";
        public const string Name = "Test User";
        public const string Role = "Admin";
        public const string Email = "testUser@nist.gov";
        
        private readonly SsoConfig _ssoConfig;

        public MockAuthenticatedUser(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock, IOptions<SsoConfig> ssoConfig)
            : base(options, logger, encoder, clock)
        {
            _ssoConfig = ssoConfig.Value;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (_ssoConfig.UseSso)
            {
                throw new ArgumentException($"{nameof(_ssoConfig.UseSso)} should not be true when working with a mock authentication handler.");
            }

            if (string.IsNullOrEmpty(_ssoConfig.ImpersonateEmail))
            {
                throw new ArgumentNullException($"{nameof(_ssoConfig.ImpersonateEmail)} should not be empty.");
            }
            
            var claims = new[] 
            {
                new Claim(ClaimTypes.NameIdentifier, NameIdentifier),
                new Claim(ClaimTypes.Name, Name),
                new Claim(ClaimTypes.Role, Role),
                new Claim(ClaimTypes.Email, _ssoConfig.ImpersonateEmail),
                new Claim(ClaimTypes.AuthenticationMethod, Scheme.Name), 
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            
            
            return await Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}