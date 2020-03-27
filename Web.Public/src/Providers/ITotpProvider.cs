namespace Web.Public.Providers
{
    public interface ITotpProvider
    {
        byte[] GetSeedFromUserCertificate(byte[] userCert);
        long GetUsedWindowFromUserCertificate(byte[] userCert);
        void SetUsedWindowFromUserCertificate(byte[] userCert, long usedWindow);
    }
}