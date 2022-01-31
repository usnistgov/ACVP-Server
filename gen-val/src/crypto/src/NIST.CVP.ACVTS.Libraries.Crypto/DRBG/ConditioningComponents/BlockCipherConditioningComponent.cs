using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.ConditioningComponents;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DRBG.ConditioningComponents
{
    public class BlockCipherConditioningComponent : IDrbgConditioningComponent
    {
        private readonly DrbgCounterAes _drbg;

        public BlockCipherConditioningComponent(DrbgCounterAes drbg)
        {
            _drbg = drbg;
        }

        public DrbgResult DerivationFunction(BitString seedMaterial, int bitsToReturn) => _drbg.BlockCipherDf(seedMaterial, bitsToReturn);
    }
}
