using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Hash.SHA2
{
    public interface ISHABase
    {
        BitString HashMessage(BitString message);
    }
}
