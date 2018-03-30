using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Hash.SHA3
{
    public interface ISHA3Wrapper
    {
        BitString HashMessage(BitString message, int digestSize, int capacity, bool XOF);
    }
}