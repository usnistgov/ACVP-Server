using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class DsaEccFactory : IDsaEccFactory
    {
        private readonly IShaFactory _shaFactory;

        public DsaEccFactory(IShaFactory shaFactory)
        {
            _shaFactory = shaFactory;
        }

        public IDsaEcc GetInstance(HashFunction hashFunction, EntropyProviderTypes entropyType = EntropyProviderTypes.Random)
        {
            return new EccDsa(_shaFactory.GetShaInstance(hashFunction), entropyType);
        }
    }
}
