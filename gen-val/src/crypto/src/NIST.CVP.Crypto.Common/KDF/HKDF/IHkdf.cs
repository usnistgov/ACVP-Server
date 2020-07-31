using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KDF.HKDF
{
    public interface IHkdf
    {
        BitString Extract(BitString salt, BitString inputKeyingMaterial);
        BitString Expand(BitString pseudoRandomKey, BitString otherInfo, int keyLength);

        KdfResult DeriveKey(BitString salt, BitString inputKeyingMaterial, BitString otherInfo, int keyLength);
    }
}