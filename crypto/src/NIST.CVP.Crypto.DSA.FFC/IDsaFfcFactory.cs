using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DSA.FFC
{
    public interface IDsaFfcFactory
    {
        IDsaFfc GetInstance(ISha sha, EntropyProviderTypes entropyType = EntropyProviderTypes.Random);
    }
}