using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.FFC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DSA.FFC
{
    public class DsaFfcFactory : IDsaFfcFactory
    {
        private readonly IShaFactory _shaFactory;

        public DsaFfcFactory(IShaFactory shaFactory)
        {
            _shaFactory = shaFactory;
        }

        public IDsaFfc GetInstance(HashFunction hashFunction, EntropyProviderTypes entropyType = EntropyProviderTypes.Random)
        {
            return new FfcDsa(_shaFactory.GetShaInstance(hashFunction), entropyType);
        }
    }
}
