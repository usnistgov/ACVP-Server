using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.ConditioningComponents
{
    public interface IHashConditioningComponentFactory
    {
        IDrbgConditioningComponent GetInstance(HashFunction hashFunction);
    }
}
