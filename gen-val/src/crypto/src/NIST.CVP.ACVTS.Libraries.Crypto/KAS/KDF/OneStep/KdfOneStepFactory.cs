using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.KMAC;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.KDF.OneStep
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

        public IKdfOneStep GetInstance(KdaOneStepAuxFunction auxFunction, bool useCounter)
        {
            switch (auxFunction)
            {
                case KdaOneStepAuxFunction.SHA1:
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA1, DigestSizes.d160)), useCounter);
                case KdaOneStepAuxFunction.SHA2_D224:
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d224)), useCounter);
                case KdaOneStepAuxFunction.SHA2_D256:
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256)), useCounter);
                case KdaOneStepAuxFunction.SHA2_D384:
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d384)), useCounter);
                case KdaOneStepAuxFunction.SHA2_D512:
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512)), useCounter);
                case KdaOneStepAuxFunction.SHA2_D512_T224:
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512t224)), useCounter);
                case KdaOneStepAuxFunction.SHA2_D512_T256:
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512t256)), useCounter);
                case KdaOneStepAuxFunction.SHA3_D224:
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d224)), useCounter);
                case KdaOneStepAuxFunction.SHA3_D256:
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d256)), useCounter);
                case KdaOneStepAuxFunction.SHA3_D384:
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d384)), useCounter);
                case KdaOneStepAuxFunction.SHA3_D512:
                    return new KdfSha(_shaFactory.GetShaInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d512)), useCounter);
                case KdaOneStepAuxFunction.HMAC_SHA1:
                case KdaOneStepAuxFunction.HMAC_SHA2_D224:
                case KdaOneStepAuxFunction.HMAC_SHA2_D256:
                case KdaOneStepAuxFunction.HMAC_SHA2_D384:
                case KdaOneStepAuxFunction.HMAC_SHA2_D512:
                case KdaOneStepAuxFunction.HMAC_SHA2_D512_T224:
                case KdaOneStepAuxFunction.HMAC_SHA2_D512_T256:
                case KdaOneStepAuxFunction.HMAC_SHA3_D224:
                case KdaOneStepAuxFunction.HMAC_SHA3_D256:
                case KdaOneStepAuxFunction.HMAC_SHA3_D384:
                case KdaOneStepAuxFunction.HMAC_SHA3_D512:
                    return new KdfHmac(_hmacFactory, _shaFactory, auxFunction, useCounter);
                case KdaOneStepAuxFunction.KMAC_128:
                    return new KdfKmac(_kmacFactory, 256, useCounter);
                case KdaOneStepAuxFunction.KMAC_256:
                    return new KdfKmac(_kmacFactory, 512, useCounter);
                default:
                    throw new ArgumentException(nameof(auxFunction));
            }
        }
    }
}
