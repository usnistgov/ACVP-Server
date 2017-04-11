using NIST.CVP.Math;

namespace NIST.CVP.Crypto.SHA2
{
    public interface ISHA
    {
        HashResult HashMessage(HashFunction hashFunction, BitString message);
    }
}
