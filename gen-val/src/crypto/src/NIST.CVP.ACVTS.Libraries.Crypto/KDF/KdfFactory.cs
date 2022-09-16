using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KDF.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.CMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.KMAC;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KDF
{
    public class KdfFactory : IKdfFactory
    {
        private readonly ICmacFactory _cmacFactory;
        private readonly IHmacFactory _hmacFactory;
        private readonly IKmacFactory _kmacFactory;

        public KdfFactory(ICmacFactory cmacFactory, IHmacFactory hmacFactory, IKmacFactory kmacFactory)
        {
            _cmacFactory = cmacFactory;
            _hmacFactory = hmacFactory;
            _kmacFactory = kmacFactory;
        }

        public IKdf GetKdfInstance(KdfModes kdfMode, MacModes macMode, CounterLocations counterLocation, int counterLength = 0)
        {
            var mac = GetMacInstance(macMode);

            switch (kdfMode)
            {
                case KdfModes.Counter:
                    return new CounterKdf(mac, counterLocation, counterLength);

                case KdfModes.Feedback:
                    return new FeedbackKdf(mac, counterLocation, counterLength);

                case KdfModes.Pipeline:
                    return new PipelineKdf(mac, counterLocation, counterLength);

                case KdfModes.Kmac:
                    return new KmacKdf(mac);
                
                default:
                    throw new ArgumentException("KDF Mode not supported");
            }
        }

        public IMac GetMacInstance(MacModes mode)
        {
            switch (mode)
            {
                case MacModes.CMAC_AES128:
                    return _cmacFactory.GetCmacInstance(CmacTypes.AES128);

                case MacModes.CMAC_AES192:
                    return _cmacFactory.GetCmacInstance(CmacTypes.AES192);

                case MacModes.CMAC_AES256:
                    return _cmacFactory.GetCmacInstance(CmacTypes.AES256);

                case MacModes.CMAC_TDES:
                    return _cmacFactory.GetCmacInstance(CmacTypes.TDES);

                case MacModes.HMAC_SHA1:
                    return _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA1, DigestSizes.d160));

                case MacModes.HMAC_SHA224:
                    return _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d224));

                case MacModes.HMAC_SHA256:
                    return _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));

                case MacModes.HMAC_SHA384:
                    return _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d384));

                case MacModes.HMAC_SHA512:
                    return _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512));

                case MacModes.HMAC_SHA_d512t224:
                    return _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512t224));

                case MacModes.HMAC_SHA_d512t256:
                    return _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512t256));

                case MacModes.HMAC_SHA3_224:
                    return _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d224));

                case MacModes.HMAC_SHA3_256:
                    return _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d256));

                case MacModes.HMAC_SHA3_384:
                    return _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d384));

                case MacModes.HMAC_SHA3_512:
                    return _hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA3, DigestSizes.d512));
                
                case MacModes.KMAC_128:
                    return _kmacFactory.GetKmacInstance(256, false);    // Capacity is 2x security strength
                
                case MacModes.KMAC_256:
                    return _kmacFactory.GetKmacInstance(512, false);    // Capacity is 2x security strength

                default:
                    throw new ArgumentException("MAC Mode not supported");
            }
        }
    }
}
