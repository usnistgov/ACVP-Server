using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Components.IKEv2
{
    public interface IIkeV2Factory
    {
        IIkeV2 GetInstance(HashFunction hashFunction);
    }
}
