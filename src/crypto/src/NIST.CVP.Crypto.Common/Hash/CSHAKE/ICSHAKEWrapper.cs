using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Hash.CSHAKE
{
    public interface ICSHAKEWrapper
    {
        BitString HashMessage(BitString message, int digestSize, int capacity, string functionName, string customization);
        BitString HashMessage(BitString message, int digestSize, int capacity);
        BitString HashMessage(BitString message, int digestSize, int capacity, string functionName, BitString customization);
    }
}
