using NIST.CVP.Math;

namespace NIST.CVP.Crypto.CTR
{
    public interface ICounter
    {
        BitString GetNextIV();
    }
}
