using ACVPCore.Results;

namespace Web.Public.Providers
{
    public interface ITotpProvider
    {
        string GenerateTotp(byte[] certRawData);
        Result ValidateTotp(byte[] certRawData, string password);
    }
}