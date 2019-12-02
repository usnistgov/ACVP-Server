using NIST.CVP.Crypto.Common.Asymmetric.RSA;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KTS;
using NIST.CVP.Math.Entropy;

namespace NIST.CVP.Crypto.KTS
{
    public class KtsFactory : IKtsFactory
    {
        private readonly IShaFactory _shaFactory;
        private readonly IRsa _rsa;
        private readonly IEntropyProviderFactory _entropyFactory;

        public KtsFactory(IShaFactory shaFactory, IRsa rsa, IEntropyProviderFactory entropyFactory)
        {
            _shaFactory = shaFactory;
            _rsa = rsa;
            _entropyFactory = entropyFactory;
        }
        
        public IRsaOaep Get(KasHashAlg hashAlg)
        {
            ISha sha = null;
            switch (hashAlg)
            {
                case KasHashAlg.SHA2_D224:
                    sha = _shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d224));
                    break;
                case KasHashAlg.SHA2_D256:
                    sha = _shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));
                    break;
                case KasHashAlg.SHA2_D384:
                    sha = _shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d384));
                    break;
                case KasHashAlg.SHA2_D512:
                    sha = _shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512));
                    break;
                case KasHashAlg.SHA2_D512_T224:
                    sha = _shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512t224));
                    break;
                case KasHashAlg.SHA2_D512_T256:
                    sha = _shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512t256));
                    break;
                case KasHashAlg.SHA3_D224:
                    sha = _shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d224));
                    break;
                case KasHashAlg.SHA3_D256:
                    sha = _shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d256));
                    break;
                case KasHashAlg.SHA3_D384:
                    sha = _shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d384));
                    break;
                case KasHashAlg.SHA3_D512:
                    sha = _shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d512));
                    break;
            }
            
            return new RsaOaep(sha, new Mgf(sha), _rsa, _entropyFactory);
        }
    }
}