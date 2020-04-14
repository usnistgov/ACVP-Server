using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NIST.CVP.ExtensionMethods;
using Web.Public.Configs;
using Web.Public.Results;

namespace Web.Public.Services
{
    public class JwtService : IJwtService
    {
        private const string SubjectKey = "sub";
        
        private readonly ILogger<JwtService> _logger;
        private readonly TimeSpan _defaultWindow;
        private readonly SecurityKey _securityKey;
        private readonly SigningCredentials _signingCredentials;
        private readonly JwtConfig _jwtOptions;
        
        public JwtService(ILogger<JwtService> logger, IOptions<JwtConfig> jwtOptions)
        {
            _logger = logger;
            _jwtOptions = jwtOptions.Value;
            _securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(_jwtOptions.SecretKey));
            _signingCredentials = new SigningCredentials(_securityKey, _jwtOptions.SignatureScheme);
            _defaultWindow = new TimeSpan(_jwtOptions.ValidWindowHours, _jwtOptions.ValidWindowMinutes, _jwtOptions.ValidWindowSeconds);
        }

        public TokenResult Create(string clientCertSubject, Dictionary<string, string> claims)
        {
            return CreateToken(clientCertSubject, claims);
        }

        public TokenResult Refresh(string clientCertSubject, string previousToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var oldToken = tokenHandler.ReadJwtToken(previousToken);

            var existingClaims = GetClaimsFromJwt(oldToken);

            // Build new token
            return CreateToken(clientCertSubject, existingClaims);
        }

        public bool IsTokenValid(string clientCertSubject, string jwtToValidate, bool validateExpiration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            
            try
            {
                tokenHandler.ValidateToken(jwtToValidate, new TokenValidationParameters()
                {
                    ValidateActor = false,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = validateExpiration,
                    IssuerSigningKey = _securityKey
                }, out _);
            }
            catch (Exception e)
            {
                _logger.LogInformation(e, $"Jwt {jwtToValidate} did not validate from {clientCertSubject}");
                return false;
            }
            
            var readJwt = tokenHandler.ReadJwtToken(jwtToValidate);
            var claims = GetClaimsFromJwt(readJwt);

            if (!claims.ContainsKey(SubjectKey))
            {
                _logger.LogWarning($"JWT {jwtToValidate} did not contain required claim '{SubjectKey}' from {clientCertSubject}.");
                return false;
            }

            if (!claims[SubjectKey].Equals(clientCertSubject))
            {
                _logger.LogWarning($"JWT {jwtToValidate} '{SubjectKey}'s did not match between ClientCert {clientCertSubject} and JWT {claims[SubjectKey]}.");
            }

            return true;
        }

        private TokenResult CreateToken(string clientCertSubject, Dictionary<string, string> claims)
        {
            var timeNow = DateTime.Now;
            var tokenHandler = new JwtSecurityTokenHandler();

            if (claims == null)
            {
                claims = new Dictionary<string, string>();
            }

            if (!claims.ContainsKey(SubjectKey))
            {
                claims.Add(SubjectKey, clientCertSubject);
            }
            
            var jwtClaims = claims.Select(s => new Claim(s.Key, s.Value)).ToArray();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                NotBefore = timeNow,
                Expires = timeNow + _defaultWindow,
                Issuer = _jwtOptions.Issuer,
                SigningCredentials = _signingCredentials,
                Subject = new ClaimsIdentity(jwtClaims),
            };

            try
            {
                var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
                return new TokenResult(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex);
                return new TokenResult("Unable to create JWT");
            }
        }
        
        protected Dictionary<string, string> GetClaimsFromJwt(JwtSecurityToken jwt)
        {
            var existingClaims = new Dictionary<string, string>();
            foreach (var claim in jwt.Claims)
            {
                existingClaims[claim.Type] = claim.Value;
            }

            return existingClaims;
        }
    }
}