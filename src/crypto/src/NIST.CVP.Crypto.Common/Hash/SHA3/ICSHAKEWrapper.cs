using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Hash.SHA3
{
    public interface ICSHAKEWrapper
    {
        BitString HashMessage(BitString message, int digestSize, int capacity, bool XOF, string functionName, string customization);
        BitString HashMessage(BitString message, int digestSize, int capacity, bool XOF);
    }
}
