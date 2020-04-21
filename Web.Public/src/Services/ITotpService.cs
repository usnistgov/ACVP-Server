using NIST.CVP.Libraries.Shared.Results;

namespace Web.Public.Services
{
    public interface ITotpService
    {
        string GenerateTotp(byte[] certRawData);
        Result ValidateTotp(byte[] certRawData, string password);
    }
}