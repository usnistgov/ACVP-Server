namespace Web.Public.Providers
{
    public interface IUserProvider
    {
        long GetUserIDFromCertificateSubject(string certSubject);
    }
}