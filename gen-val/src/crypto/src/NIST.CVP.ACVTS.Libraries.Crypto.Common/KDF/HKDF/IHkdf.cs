using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.HKDF
{
    public interface IHkdf
    {
        BitString Extract(BitString salt, BitString inputKeyingMaterial);
        BitString Expand(BitString pseudoRandomKey, BitString otherInfo, int keyLengthBytes);

        KdfResult DeriveKey(BitString salt, BitString inputKeyingMaterial, BitString otherInfo, int keyLengthBytes);
    }
}
