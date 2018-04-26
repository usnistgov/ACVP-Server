using System;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KDF;
using NIST.CVP.Crypto.Common.KDF.Enums;
using NIST.CVP.Crypto.Common.MAC;
using NIST.CVP.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.SHAWrapper;

namespace NIST.CVP.Crypto.KDF
{
    public class KdfFactory : IKdfFactory
    {
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
            var cmacFactory = new CmacFactory();
            var hmacFactory = new HmacFactory(new ShaFactory());

            switch (mode)
            {
                case MacModes.CMAC_AES128:
                    return cmacFactory.GetCmacInstance(CmacTypes.AES128);

                case MacModes.CMAC_AES192:
                    return cmacFactory.GetCmacInstance(CmacTypes.AES192);
                
                case MacModes.CMAC_AES256:
                    return cmacFactory.GetCmacInstance(CmacTypes.AES256);

                case MacModes.CMAC_TDES:
                    return cmacFactory.GetCmacInstance(CmacTypes.TDES);

                case MacModes.HMAC_SHA1:
                    return hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA1, DigestSizes.d160));

                case MacModes.HMAC_SHA224:
                    return hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d224));

                case MacModes.HMAC_SHA256:
                    return hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d256));

                case MacModes.HMAC_SHA384:
                    return hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d384));

                case MacModes.HMAC_SHA512:
                    return hmacFactory.GetHmacInstance(new HashFunction(ModeValues.SHA2, DigestSizes.d512));

                default:
                    throw new ArgumentException("MAC Mode not supported");
            }
        }
    }
}
