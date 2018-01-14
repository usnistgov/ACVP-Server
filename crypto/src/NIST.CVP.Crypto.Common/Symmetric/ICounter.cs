using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Symmetric
{
    public interface ICounter
    {
        BitString GetNextIV();
    }
}
