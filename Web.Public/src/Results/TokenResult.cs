using System.IdentityModel.Tokens.Jwt;
using ACVPCore.Results;

namespace Web.Public.Results
{
    public class TokenResult : Result
    {
        public string Token => _handler?.WriteToken(_token);

        private readonly JwtSecurityToken _token;
        private readonly JwtSecurityTokenHandler _handler;

        public TokenResult(JwtSecurityToken token)
        {
            _token = token;
            _handler = new JwtSecurityTokenHandler();
        }
        
        public TokenResult(string errorMessage) : base(errorMessage) { }
    }
}