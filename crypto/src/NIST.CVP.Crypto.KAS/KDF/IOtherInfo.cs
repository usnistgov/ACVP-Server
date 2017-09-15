using NIST.CVP.Generation.Core;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KDF
{
    public interface IOtherInfo
    {
        BitString GetOtherInfo();
    }
}