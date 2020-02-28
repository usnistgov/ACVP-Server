using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using ACVPCore.Results;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Web.Public.Results;

namespace Web.Public.Services
{
    public class JwtService : IJwtService
    {
        private readonly string _secretKey;
        private readonly TimeSpan _defaultWindow;
        private readonly string _issuer;
        private readonly SigningCredentials _signingCredentials;
        
        public JwtService()
        {
            _secretKey =
                "J6k2eVCTXDp5b97u6gNH5GaaqHDxCmzz2wv3PRPFRsuW2UavK8LGPRauC4VSeaetKTMtVmVzAC8fh8Psvp8PFybEvpYnULHfRpM8TA2an7GFehrLLvawVJdSRqh2unCnWehhh2SJMMg5bktRRapA8EGSgQUV8TCafqdSEHNWnGXTjjsMEjUpaxcADDNZLSYPMyPSfp6qe5LMcd5S9bXH97KeeMGyZTS2U8gp3LGk2kH4J4F3fsytfpe9H9qKwgjb";
            _issuer = "NIST ACVP ENVIRONMENT";
            _defaultWindow = new TimeSpan(0, 30, 0);
            _signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.Default.GetBytes(_secretKey)), SecurityAlgorithms.HmacSha256Signature);
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
                Issuer = _issuer,
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

        // TODO incomplete
        public Result Validate(string tokenString)
        {
            var validationParameters = new TokenValidationParameters
            {
                 
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var claims = tokenHandler.ValidateToken(tokenString, validationParameters, out var validatedToken);
                
                return new Result();
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
                return new Result("Unable to validate JWT");
            }
        }

        public TokenResult AddClaims()
        {
            return new TokenResult("Unable to add claims to JWT");
        }
    }
}