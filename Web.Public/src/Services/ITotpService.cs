using NIST.CVP.Libraries.Shared.Results;

namespace Web.Public.Services
{
    public interface ITotpService
    {
        string GenerateTotp(string userCertSubject);
        Result ValidateTotp(string userCertSubject, string password);
    }
}