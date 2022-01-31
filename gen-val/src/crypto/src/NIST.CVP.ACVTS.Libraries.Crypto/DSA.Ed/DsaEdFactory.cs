using NIST.CVP.ACVTS.Libraries.Crypto.Common.Asymmetric.DSA.Ed;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Math.Entropy;

namespace NIST.CVP.ACVTS.Libraries.Crypto.DSA.Ed
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
