using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Hash.CSHAKE
{
    public interface ICSHAKEWrapper
    {
        BitString HashMessage(BitString message, int digestLength, int capacity, string customization, string functionName);
        BitString HashMessage(BitString message, int digestLength, int capacity);
        BitString HashMessage(BitString message, int digestLength, int capacity, BitString customization, string functionName);
    }
}
