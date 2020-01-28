using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.CMAC;
using NIST.CVP.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.Crypto.Common.MAC.HMAC;
using System;

namespace NIST.CVP.Crypto.KDF
{
    public class KdfFactory : IKdfFactory
    {
        private readonly ICmacFactory _cmacFactory;
        private readonly IHmacFactory _hmacFactory;

        public KdfFactory(ICmacFactory cmacFactory, IHmacFactory hmacFactory)
        {
            _cmacFactory = cmacFactory;
            _hmacFactory = hmacFactory;
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

                default:
                    throw new ArgumentException("MAC Mode not supported");
            }
        }
    }
}
