namespace Web.Public.Providers
{
    public interface IUserProvider
    {
        long GetUserIDFromCertificate(byte[] certRawData);
    }
}