namespace Web.Public.Providers
{
    public interface ITotpProvider
    {
        byte[] GetSeedFromUserCertificateSubject(string userCertSubject);
        long GetUsedWindowFromUserCertificateSubject(string userCertSubject);
        void SetUsedWindowFromUserCertificateSubject(string userCertSubject, long usedWindow);
    }
}