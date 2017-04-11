using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SHA3
{
    public interface ISHA3
    {
        HashResult HashMessage(HashFunction hashFunction, BitString message);
    }
}
