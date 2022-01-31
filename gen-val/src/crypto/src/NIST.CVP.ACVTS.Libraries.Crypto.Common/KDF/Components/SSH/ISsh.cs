using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.SSH
{
    public interface ISsh
    {
        KdfResult DeriveKey(BitString k, BitString h, BitString sessionId);
    }
}
