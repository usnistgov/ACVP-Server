using ACVPCore.Results;
using Web.Public.Results;

namespace Web.Public.Services
{
    public interface IJwtService
    {
        TokenResult Create();
        TokenResult Refresh(string previousToken);
        TokenResult AddClaims();
    }
}