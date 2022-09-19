using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.cSHAKE
{
    public interface IcSHAKE
    {
        HashResult HashMessage(HashFunction hashFunction, BitString message, string customization, string functionName = "");
        HashResult HashMessage(HashFunction hashFunction, BitString message, BitString customization, string functionName = "");
    }
}
