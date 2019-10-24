using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC;
using NIST.CVP.Crypto.Common.Asymmetric.DSA.ECC.Enums;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.MAC.HMAC;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.DSA.ECC
{
    public class DsaEccFactory : IDsaEccFactory
    {
        private readonly IHmacFactory _hmacFactory;
        private readonly IShaFactory _shaFactory;
        private readonly IEccNonceProviderFactory _nonceProviderFactory;
        private readonly IEntropyProviderFactory _entropyProviderFactory;

        public DsaEccFactory(IShaFactory shaFactory, IHmacFactory hmacFactory, IEccNonceProviderFactory nonceProviderFactory, IEntropyProviderFactory entropyProviderFactory)
        {
            _shaFactory = shaFactory;
            _hmacFactory = hmacFactory;
            _nonceProviderFactory = nonceProviderFactory;
            _entropyProviderFactory = entropyProviderFactory;
        }

        /// <summary>
        /// Takes in completed entropy provider and returns an ECDSA Key Generator
        /// </summary>
        /// <param name="keyEntropy">Completed entropy provider (BigInteger)</param>
        /// <returns></returns>
        public IDsaEcc GetInstanceForKeys(IEntropyProvider keyEntropy)
        {
            return new EccDsa(keyEntropy);
        }

        public IDsaEcc GetInstanceForKeyVerification()
        {
            return new EccDsa();
        }

        /// <summary>
        /// Takes in a hash function, and nonce provider, and optional entropy provider (only used for 'random' nonce)
        /// </summary>
        /// <param name="hashFunction"></param>
        /// <param name="nonceProviderTypes"></param>
        /// <param name="entropyProvider"></param>
        /// <returns></returns>
        public IDsaEcc GetInstanceForSignatures(HashFunction hashFunction, NonceProviderTypes nonceProviderTypes, IEntropyProvider entropyProvider = null)
        {
            var sha = _shaFactory.GetShaInstance(hashFunction);
            var hmac = _hmacFactory.GetHmacInstance(hashFunction);
            var nonceProvider = _nonceProviderFactory.GetNonceProvider(nonceProviderTypes, hmac, entropyProvider);

            return new EccDsa(sha, nonceProvider);
        }

        /// <summary>
        /// Takes in a hash function and returns an ECDSA instance for Signature Verification
        /// </summary>
        /// <param name="hashFunction"></param>
        /// <returns></returns>
        public IDsaEcc GetInstanceForVerification(HashFunction hashFunction)
        {
            var sha = _shaFactory.GetShaInstance(hashFunction);

            return new EccDsa(sha);
        }

        public IDsaEcc GetInstance(HashFunction hashFunction, EntropyProviderTypes entropyType = EntropyProviderTypes.Random)
        {
            // KAS ECC is using the base IDsa function that relies on a hash function and entropy type.
            // Kas ECC should be the only method using this function, but I didn't want to mark it obsolete,
            // as the FFC KAS also uses this function from the base IDsa interface.
            return new EccDsa(_shaFactory.GetShaInstance(hashFunction), _entropyProviderFactory.GetEntropyProvider(entropyType));
        }
    }
}
