using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Web.Public.Configs;
using Web.Public.Results;

namespace Web.Public.Services
{
    public class JwtService : IJwtService
    {
        private readonly TimeSpan _defaultWindow;
        private readonly SigningCredentials _signingCredentials;
        private readonly JwtConfig _jwtOptions;
        
        public JwtService(IOptions<JwtConfig> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
            _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.Default.GetBytes(_jwtOptions.SecretKey)), _jwtOptions.SignatureScheme);
            _defaultWindow = new TimeSpan(_jwtOptions.ValidWindowHours, _jwtOptions.ValidWindowMinutes, _jwtOptions.ValidWindowSeconds);
        }
        
        public TokenResult Create()
        {
            return CreateToken();
        }

        // TODO needs testing, no claims are available to add yet because no other resources exist
        // TODO Should use validate/create not something new
        public TokenResult Refresh(string previousToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var oldToken = tokenHandler.ReadJwtToken(previousToken);

            // Transfer old claims
            var existingClaims = new Dictionary<string, object>();
            foreach (var claim in oldToken.Claims)
            {
                existingClaims[claim.Type] = claim.Value;
            }
            
            // Build new token
            return CreateToken(existingClaims);
        }

        private TokenResult CreateToken(IDictionary<string, object> claims = null)
        {
            var timeNow = DateTime.Now;
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                NotBefore = timeNow,
                Expires = timeNow + _defaultWindow,
                Issuer = _jwtOptions.Issuer,
                SigningCredentials = _signingCredentials,
                Claims = claims
            };

            try
            {
                var token = tokenHandler.CreateJwtSecurityToken(tokenDescriptor);
                return new TokenResult(token);
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
                return new TokenResult("Unable to create JWT");
            }
        }

        public TokenResult AddClaims()
        {
            return new TokenResult("Unable to add claims to JWT");
        }
    }
}