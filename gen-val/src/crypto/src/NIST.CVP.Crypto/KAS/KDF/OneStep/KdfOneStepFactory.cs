﻿using System;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.MAC.HMAC;
using NIST.CVP.Crypto.Common.MAC.KMAC;

namespace NIST.CVP.Crypto.KAS.KDF.OneStep
{
    public class KdfOneStepFactory : IKdfOneStepFactory
    {
        private readonly IShaFactory _shaFactory;
        private readonly IHmacFactory _hmacFactory;
        private readonly IKmacFactory _kmacFactory;

        public KdfOneStepFactory(IShaFactory shaFactory, IHmacFactory hmacFactory, IKmacFactory kmacFactory)
        {
            _shaFactory = shaFactory;
            _hmacFactory = hmacFactory;
            _kmacFactory = kmacFactory;
        }

        public IKdfOneStep GetInstance(KasKdfOneStepAuxFunction auxFunction)
        {
            switch (auxFunction)
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
                    return new KdfHmac(_hmacFactory, _shaFactory, auxFunction);
                case KasKdfOneStepAuxFunction.KMAC_128:
                    return new KdfKmac(_kmacFactory, 128);
                case KasKdfOneStepAuxFunction.KMAC_256:
                    return new KdfKmac(_kmacFactory, 256);
                default:
                    throw new ArgumentException(nameof(auxFunction));
            }
        }
    }
}