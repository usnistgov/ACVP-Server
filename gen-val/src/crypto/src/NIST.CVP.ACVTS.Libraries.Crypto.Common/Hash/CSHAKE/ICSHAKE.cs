using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.CSHAKE
{
    public interface ICSHAKE
    {
        HashResult HashMessage(HashFunction hashFunction, BitString message, string customization, string functionName = "");
        HashResult HashMessage(HashFunction hashFunction, BitString message, BitString customization, string functionName = "");
    }
}
