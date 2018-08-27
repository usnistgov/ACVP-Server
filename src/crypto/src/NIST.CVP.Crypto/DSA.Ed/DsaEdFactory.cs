using NIST.CVP.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DSA.Ed
{
    public class DsaEdFactory : IDsaEdFactory
    {
        /// <summary>
        /// hashFunction should just be null
        /// </summary>
        /// <param name="hashFunction"></param>
        /// <param name="entropyType"></param>
        /// <returns></returns>
        public IDsaEd GetInstance(HashFunction hashFunction, EntropyProviderTypes entropyType = EntropyProviderTypes.Random)
        {
            return new EdDsa(entropyType);
        }
    }
}
