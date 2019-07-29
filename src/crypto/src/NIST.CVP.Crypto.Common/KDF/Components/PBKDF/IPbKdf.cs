using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KDF.Components.PBKDF
{
    public interface IPbKdf
    {
        KdfResult DeriveKey(BitString salt, string password, int c, int keyLen);
    }
}