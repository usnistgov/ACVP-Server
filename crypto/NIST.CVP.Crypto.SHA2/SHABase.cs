using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SHA2
{
    public abstract class SHABase
    {
        public abstract BitString HashMessage(BitString message);
    }
}
