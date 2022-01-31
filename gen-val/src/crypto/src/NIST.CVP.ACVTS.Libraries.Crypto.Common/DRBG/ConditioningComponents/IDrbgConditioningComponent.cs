using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG.ConditioningComponents
{
    public interface IDrbgConditioningComponent
    {
        DrbgResult DerivationFunction(BitString seedMaterial, int bitsToReturn);
    }
}
