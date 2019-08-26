using System;
using System.Numerics;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KDF.KdfOneStep;
using NIST.CVP.Crypto.Common.MAC.HMAC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KDF
{
    public class KdfHmac : KdfBase
    {
        private readonly IHmac _hmac;
        private readonly ISha _sha;
        
        public KdfHmac(IHmacFactory hmacFactory, IShaFactory shaFactory, OneStepConfiguration config)
        {
            HashFunction hashFunction = null;
            switch (config.AuxFunction.AuxFunctionName)
            {
                case KasKdfOneStepAuxFunction.HMAC_SHA2_D224:
                    hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d224);
                    break;
                case KasKdfOneStepAuxFunction.HMAC_SHA2_D256:
                    hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d256);
                    break;
                case KasKdfOneStepAuxFunction.HMAC_SHA2_D384:
                    hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d384);
                    break;
                case KasKdfOneStepAuxFunction.HMAC_SHA2_D512:
                    hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d512);
                    break;
                case KasKdfOneStepAuxFunction.HMAC_SHA2_D512_T224:
                    hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d512t224);
                    break;
                case KasKdfOneStepAuxFunction.HMAC_SHA2_D512_T256:
                    hashFunction = new HashFunction(ModeValues.SHA2, DigestSizes.d512t256);
                    break;
                case KasKdfOneStepAuxFunction.HMAC_SHA3_D224:
                    hashFunction = new HashFunction(ModeValues.SHA3, DigestSizes.d224);
                    break;
                case KasKdfOneStepAuxFunction.HMAC_SHA3_D256:
                    hashFunction = new HashFunction(ModeValues.SHA3, DigestSizes.d256);
                    break;
                case KasKdfOneStepAuxFunction.HMAC_SHA3_D384:
                    hashFunction = new HashFunction(ModeValues.SHA3, DigestSizes.d384);
                    break;
                case KasKdfOneStepAuxFunction.HMAC_SHA3_D512:
                    hashFunction = new HashFunction(ModeValues.SHA3, DigestSizes.d512);
                    break;
                default:
                    throw new ArgumentException(nameof(config.AuxFunction.AuxFunctionName));
            }

            _hmac = hmacFactory.GetHmacInstance(hashFunction);
            _sha = shaFactory.GetShaInstance(hashFunction);
        }
        
        protected override int OutputLength => _sha.HashFunction.OutputLen;
        protected override BigInteger MaxInputLength => _sha.HashFunction.MaxMessageLen;
        protected override BitString H(BitString message, BitString salt)
        {
            if (salt == null || salt.BitLength == 0)
            {
                throw new ArgumentNullException(nameof(salt));
            }
            
            return _hmac.Generate(salt, message, OutputLength).Mac;
        }
    }
}