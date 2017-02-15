using NIST.CVP.Math;

namespace NIST.CVP.Generation.SHA2
{
    public abstract class SHABase
    {
        public abstract BitString HashMessage(BitString message);
    }
}
