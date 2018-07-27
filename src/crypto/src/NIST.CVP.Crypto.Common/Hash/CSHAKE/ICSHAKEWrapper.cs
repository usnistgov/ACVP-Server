using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Hash.CSHAKE
{
    public interface ICSHAKEWrapper
    {
        BitString HashMessage(BitString message, int digestLength, int capacity, string functionName, string customization);
        BitString HashMessage(BitString message, int digestLength, int capacity);
        BitString HashMessage(BitString message, int digestLength, int capacity, string functionName, BitString customization);
    }
}
