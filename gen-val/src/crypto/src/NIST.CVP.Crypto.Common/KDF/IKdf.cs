using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KDF
{
    public interface IKdf
    {
        KdfResult DeriveKey(BitString kI, BitString fixedData, int len, BitString iv = null, int breakLocation = 0);
    }
}
