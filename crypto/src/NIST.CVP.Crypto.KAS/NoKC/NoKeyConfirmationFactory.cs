using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.KAS.Helpers;
using NIST.CVP.Crypto.KAS.Scheme;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Math;
using ModeValues = NIST.CVP.Crypto.SHAWrapper.ModeValues;

namespace NIST.CVP.Crypto.KAS.NoKC
{
    public class NoKeyConfirmationFactory : INoKeyConfirmationFactory
    {
        public INoKeyConfirmation GetInstance(INoKeyConfirmationParameters parameters)
        {
            switch (parameters.KeyAgreementMacType)
            {
                case KeyAgreementMacType.AesCcm:
                    return new NoKeyConfirmationAesCcm(parameters, new AES_CCM.AES_CCM(new AES_CCMInternals(), new RijndaelFactory(new RijndaelInternals())));
                case KeyAgreementMacType.CmacAes:
                    return new NoKeyConfirmationCmac(parameters, new CmacAes(new RijndaelFactory(new RijndaelInternals())));
                case KeyAgreementMacType.HmacSha2D224:
                case KeyAgreementMacType.HmacSha2D256:
                case KeyAgreementMacType.HmacSha2D384:
                case KeyAgreementMacType.HmacSha2D512:
                    HmacFactory hmacFactory = new HmacFactory(new ShaFactory());
                    ModeValues modeValue = ModeValues.SHA2;
                    DigestSizes digestSize = DigestSizes.NONE;
                    EnumMapping.GetHashFunctionOptions(parameters.KeyAgreementMacType, ref modeValue, ref digestSize);
                    return new NoKeyConfirmationHmac(parameters, hmacFactory.GetHmacInstance(new HashFunction(modeValue, digestSize)));
                default:
                     throw new ArgumentException(nameof(parameters.KeyAgreementMacType));
            }
        }
    }
}
