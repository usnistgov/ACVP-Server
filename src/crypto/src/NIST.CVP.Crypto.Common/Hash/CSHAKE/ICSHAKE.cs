using NIST.CVP.Math;
using NIST.CVP.Crypto.Common.Hash.SHA3;

namespace NIST.CVP.Crypto.Common.Hash.CSHAKE
{
    public interface ICSHAKE
    {
        HashResult HashMessage(HashFunction hashFunction, BitString message, string functionName, string customization);
        HashResult HashMessage(HashFunction hashFunction, BitString message);
    }
}
