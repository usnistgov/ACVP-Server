using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.KAS.KDF
{
    public interface IOtherInfo
    {
        BitString GetOtherInfo();
    }
}