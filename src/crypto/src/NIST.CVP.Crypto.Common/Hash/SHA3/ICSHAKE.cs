using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Hash.SHA3
{
    public interface ICSHAKE
    {
        HashResult HashMessage(HashFunction hashFunction, BitString message, string functionName, string customization);
        HashResult HashMessage(HashFunction hashFunction, BitString message);
    }
}
