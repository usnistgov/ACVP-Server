using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA2
{
    public interface ISHABase
    {
        BitString HashMessage(BitString message);
    }
}
