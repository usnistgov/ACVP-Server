using System;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.MAC.HMAC;

namespace NIST.CVP.Crypto.KAS.KDF
{
    public class KdfFactory : IKdfOneStepFactory
    {
        private readonly IShaFactory _shaFactory;
        private readonly IHmacFactory _hmacFactory;

        public KdfFactory(IShaFactory shaFactory, IHmacFactory hmacFactory)
        {
            _shaFactory = shaFactory;
            _hmacFactory = hmacFactory;
        }

        public IKdfOneStep GetInstance(KdfHashMode kdfHashMode, HashFunction hashFunction)
        {
            var sha = _shaFactory.GetShaInstance(hashFunction);

            switch (kdfHashMode)
            {
                    case KdfHashMode.Sha:
                        return new KdfSha(sha);
                    default:
                    throw new ArgumentException(nameof(kdfHashMode));
            }
        }

        public IKdfOneStep GetInstance(OneStepConfiguration config)
        {
            switch (config.AuxFunction.AuxFunctionName)
            {
                case KasKdfOneStepAuxFunction.SHA2_D224:
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d224)));
                case KasKdfOneStepAuxFunction.SHA2_D256:
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256)));
                case KasKdfOneStepAuxFunction.SHA2_D384:
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d384)));
                case KasKdfOneStepAuxFunction.SHA2_D512:
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512)));
                case KasKdfOneStepAuxFunction.SHA2_D512_T224:
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512t224)));
                case KasKdfOneStepAuxFunction.SHA2_D512_T256:
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512t224)));
                case KasKdfOneStepAuxFunction.SHA3_D224: 
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d224)));
                case KasKdfOneStepAuxFunction.SHA3_D256:
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d224)));
                case KasKdfOneStepAuxFunction.SHA3_D384:
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d224)));
                case KasKdfOneStepAuxFunction.SHA3_D512:
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d224)));
                case KasKdfOneStepAuxFunction.HMAC_SHA2_D224:
                case KasKdfOneStepAuxFunction.HMAC_SHA2_D256:
                case KasKdfOneStepAuxFunction.HMAC_SHA2_D384:
                case KasKdfOneStepAuxFunction.HMAC_SHA2_D512:
                case KasKdfOneStepAuxFunction.HMAC_SHA2_D512_T224:
                case KasKdfOneStepAuxFunction.HMAC_SHA2_D512_T256:
                case KasKdfOneStepAuxFunction.HMAC_SHA3_D224:
                case KasKdfOneStepAuxFunction.HMAC_SHA3_D256:
                case KasKdfOneStepAuxFunction.HMAC_SHA3_D384:
                case KasKdfOneStepAuxFunction.HMAC_SHA3_D512:
                    return new KdfHmac(_hmacFactory, _shaFactory, config);
                default:
                    throw new ArgumentException(nameof(config.AuxFunction.AuxFunctionName));
            }
        }
    }
}