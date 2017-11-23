using System;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.NoKC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using ModeValues = NIST.CVP.Crypto.SHAWrapper.ModeValues;

namespace NIST.CVP.Generation.KAS.Fakes
{
    public class FakeNoKeyConfirmationFactory_BadMacData : INoKeyConfirmationFactory
    {
        public INoKeyConfirmation GetInstance(INoKeyConfirmationParameters parameters)
        {
            switch (parameters.KeyAgreementMacType)
            {
                case KeyAgreementMacType.AesCcm:
                    return new FakeNoKeyConfirmationAesCcm_BadMacData(parameters, new AES_CCM(new AES_CCMInternals(), new RijndaelFactory(new RijndaelInternals())));
                case KeyAgreementMacType.CmacAes:
                    return new FakeNoKeyConfirmationCmac_BadMacData(parameters, new CmacAes(new RijndaelFactory(new RijndaelInternals())));
                case KeyAgreementMacType.HmacSha2D224:
                case KeyAgreementMacType.HmacSha2D256:
                case KeyAgreementMacType.HmacSha2D384:
                case KeyAgreementMacType.HmacSha2D512:
                    HmacFactory hmacFactory = new HmacFactory(new ShaFactory());
                    ModeValues modeValue = ModeValues.SHA2;
                    DigestSizes digestSize = DigestSizes.NONE;
                    EnumMapping.GetHashFunctionOptions(parameters.KeyAgreementMacType, ref modeValue, ref digestSize);
                    return new FakeNoKeyConfirmationHmac_BadMacData(parameters, hmacFactory.GetHmacInstance(new HashFunction(modeValue, digestSize)));
                default:
                    throw new ArgumentException(nameof(parameters.KeyAgreementMacType));
            }
        }

        internal class FakeNoKeyConfirmationAesCcm_BadMacData : NoKeyConfirmationAesCcm
        {
            public FakeNoKeyConfirmationAesCcm_BadMacData(INoKeyConfirmationParameters parameters, IAES_CCM aes_ccm)
                : base(parameters, aes_ccm) { }

            protected override BitString Mac(BitString macData)
            {
                macData[0] += 2;

                return base.Mac(macData);
            }
        }

        internal class FakeNoKeyConfirmationCmac_BadMacData : NoKeyConfirmationCmac
        {
            public FakeNoKeyConfirmationCmac_BadMacData(INoKeyConfirmationParameters parameters, ICmac cmacAes)
                : base(parameters, cmacAes) { }

            protected override BitString Mac(BitString macData)
            {
                macData[0] += 2;

                return base.Mac(macData);
            }
        }

        internal class FakeNoKeyConfirmationHmac_BadMacData : NoKeyConfirmationHmac
        {
            public FakeNoKeyConfirmationHmac_BadMacData(INoKeyConfirmationParameters parameters, IHmac hmac)
                : base(parameters, hmac) { }

            protected override BitString Mac(BitString macData)
            {
                macData[0] += 2;

                return base.Mac(macData);
            }
        }
    }
}