using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Web.Public.Results;

namespace Web.Public.Services
{
    public interface IJwtService
    {
        TokenResult Create(string clientCertSubject, Dictionary<string, string> claims);
        TokenResult Refresh(string clientCertSubject, string previousToken);
        bool IsTokenValid(string clientCertSubject, string jwtToValidate, bool validateExpiration);
    }
}