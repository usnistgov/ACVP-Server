using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS.Enums;
using NIST.CVP.ACVTS.Libraries.Math;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.LMS
{
    public interface IHssFactory
    {
        IHss GetInstance(int layers, LmsType[] lmsTypes, LmotsType[] lmotsTypes,
            EntropyProviderTypes entropyType = EntropyProviderTypes.Random, BitString seed = null, BitString rootI = null);
    }
}
