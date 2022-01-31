using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric
{
    public interface ICounter
    {
        BitString GetNextIV();
    }
}
