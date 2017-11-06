using System;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.KC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;

namespace NIST.CVP.Generation.KAS.FFC.Fakes
{
    public class FakeKeyConfirmationFactory_BadMacData : IKeyConfirmationFactory
    {
        public IKeyConfirmation GetInstance(IKeyConfirmationParameters parameters)
        {
            switch (parameters.MacType)
            {
                case KeyAgreementMacType.AesCcm:
                    return new FakeKeyConfirmationAesCcm_BadMacData(parameters, new AES_CCM(new AES_CCMInternals(), new RijndaelFactory(new RijndaelInternals())));
                case KeyAgreementMacType.CmacAes:
                    return new FakeKeyConfirmationCmac_BadMacData(parameters, new CmacAes(new RijndaelFactory(new RijndaelInternals())));
                case KeyAgreementMacType.HmacSha2D224:
                case KeyAgreementMacType.HmacSha2D256:
                case KeyAgreementMacType.HmacSha2D384:
                case KeyAgreementMacType.HmacSha2D512:
                    HmacFactory hmacFactory = new HmacFactory(new ShaFactory());
                    Crypto.SHAWrapper.ModeValues modeValue = Crypto.SHAWrapper.ModeValues.SHA2;
                    DigestSizes digestSize = DigestSizes.NONE;
                    EnumMapping.GetHashFunctionOptions(parameters.MacType, ref modeValue, ref digestSize);
                    return new FakeKeyConfirmationHmac_BadMacData(parameters, hmacFactory.GetHmacInstance(new HashFunction(modeValue, digestSize)));
                default:
                    throw new ArgumentException(nameof(parameters.MacType));
            }
        }

        internal class FakeKeyConfirmationAesCcm_BadMacData : KeyConfirmationAesCcm
        {
            public FakeKeyConfirmationAesCcm_BadMacData(IKeyConfirmationParameters parameters, IAES_CCM aes_ccm)
                : base(aes_ccm, parameters) { }

            protected override BitString Mac(BitString macData)
            {
                macData[0] += 2;

                return base.Mac(macData);
            }
        }

        internal class FakeKeyConfirmationCmac_BadMacData : KeyConfirmationCmac
        {
            public FakeKeyConfirmationCmac_BadMacData(IKeyConfirmationParameters parameters, ICmac cmacAes)
                : base(cmacAes, parameters) { }

            protected override BitString Mac(BitString macData)
            {
                macData[0] += 2;

                return base.Mac(macData);
            }
        }

        internal class FakeKeyConfirmationHmac_BadMacData : KeyConfirmationHmac
        {
            public FakeKeyConfirmationHmac_BadMacData(IKeyConfirmationParameters parameters, IHmac hmac)
                : base(hmac, parameters) { }

            protected override BitString Mac(BitString macData)
            {
                macData[0] += 2;

                return base.Mac(macData);
            }
        }
    }
}