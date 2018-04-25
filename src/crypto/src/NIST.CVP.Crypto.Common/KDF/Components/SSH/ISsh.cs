using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KDF.Components.SSH
{
    public interface ISsh
    {
        KdfResult DeriveKey(BitString k, BitString h, BitString sessionId);
    }
}
