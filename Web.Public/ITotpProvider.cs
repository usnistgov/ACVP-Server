namespace Web.Public
{
    public interface ITotpProvider
    {
        string GenerateTotp(byte[] seed);
        (bool success, long computedWindow) ValidateTotp(byte[] seed, string password);
    }
}