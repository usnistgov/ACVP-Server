using NIST.CVP.Math;

namespace NIST.CVP.Crypto.Common.Hash.SHA3
{
    public interface ISHA3
    {
        HashResult HashMessage(HashFunction hashFunction, BitString message);
    }
}
