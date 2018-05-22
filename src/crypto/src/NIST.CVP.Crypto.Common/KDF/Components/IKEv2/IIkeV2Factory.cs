using NIST.CVP.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.Crypto.Common.KDF.Components.IKEv2
{
    public interface IIkeV2Factory
    {
        IIkeV2 GetInstance(HashFunction hashFunction);
    }
}