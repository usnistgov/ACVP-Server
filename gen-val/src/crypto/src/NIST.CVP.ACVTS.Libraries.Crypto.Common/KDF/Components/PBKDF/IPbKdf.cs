using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.PBKDF
{
    public interface IPbKdf
    {
        KdfResult DeriveKey(BitString salt, string password, int c, int keyLen);
    }
}
