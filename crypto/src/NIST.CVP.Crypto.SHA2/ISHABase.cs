using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SHA2
{
    public interface ISHABase
    {
        BitString HashMessage(BitString message);
    }
}
