using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.ConditioningComponents;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DRBG.ConditioningComponents
{
    public class HashConditioningComponent : IDrbgConditioningComponent
    {
        private readonly DrbgHash _drbg;

        public HashConditioningComponent(DrbgHash drbg)
        {
            _drbg = drbg;
        }

        public DrbgResult DerivationFunction(BitString seedMaterial, int bitsToReturn) => _drbg.Hash_Df(seedMaterial, bitsToReturn);
    }
}
