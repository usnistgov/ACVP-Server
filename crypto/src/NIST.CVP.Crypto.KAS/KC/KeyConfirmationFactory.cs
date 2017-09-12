using System;
using System.Linq;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.CMAC.Enums;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.KAS.Enums;
using NIST.CVP.Crypto.SHAWrapper;
using DigestSizes = NIST.CVP.Crypto.SHAWrapper.DigestSizes;
using HashFunction = NIST.CVP.Crypto.SHAWrapper.HashFunction;
using ModeValues = NIST.CVP.Crypto.SHAWrapper.ModeValues;

namespace NIST.CVP.Crypto.KAS.KC
{
    public class KeyConfirmationFactory : IKeyConfirmationFactory
    {
        public IKeyConfirmation GetInstance(IKeyConfirmationParameters parameters)
        {
            switch (parameters.MacType)
            {
                case KeyAgreementMacType.AesCcm:
                    ConfirmKeyLengthAesCcm(parameters.KeyLength);
                    return new KeyConfirmationAesCcm(
                        new AES_CCM.AES_CCM(
                            new AES_CCMInternals(),
                            new RijndaelFactory(
                                new RijndaelInternals()
                            )
                        ),
                        parameters
                    );
                case KeyAgreementMacType.CmacAes:
                    var cmacEnum = MapCmacEnum(parameters.KeyLength);
                    CmacFactory cmacFactory = new CmacFactory();

                    return new KeyConfirmationCmac(
                        cmacFactory.GetCmacInstance(cmacEnum), parameters
                    );
                case KeyAgreementMacType.HmacSha2D224:
                case KeyAgreementMacType.HmacSha2D256:
                case KeyAgreementMacType.HmacSha2D384:
                case KeyAgreementMacType.HmacSha2D512:
                    var hashFunction = GetHashFunction(parameters.MacType);
                    HmacFactory hmacFactory = new HmacFactory(new ShaFactory());

                    return new KeyConfirmationHmac(
                        hmacFactory.GetHmacInstance(hashFunction), parameters
                    );
                default:
                    throw new ArgumentException(nameof(parameters.MacType));
            }
        }

        private void ConfirmKeyLengthAesCcm(int parametersKeyLength)
        {
            var validKeys = new int[] { 128, 192, 256 };

            if (!validKeys.Contains(parametersKeyLength))
            {
                throw new ArgumentException(nameof(parametersKeyLength));
            }
        }
        
        private CmacTypes MapCmacEnum(int parametersKeyLength)
        {
            switch (parametersKeyLength)
            {
                case 128:
                    return CmacTypes.AES128;
                case 192:
                    return CmacTypes.AES192;
                case 256:
                    return CmacTypes.AES256;
                default:
                    throw new ArgumentException($"Invalid {parametersKeyLength}");
            }
        }

        private HashFunction GetHashFunction(KeyAgreementMacType parametersMacType)
        {
            switch (parametersMacType)
            {
                case KeyAgreementMacType.HmacSha2D224:
                    return new HashFunction(ModeValues.SHA2, DigestSizes.d224);
                case KeyAgreementMacType.HmacSha2D256:
                    return new HashFunction(ModeValues.SHA2, DigestSizes.d256);
                case KeyAgreementMacType.HmacSha2D384:
                    return new HashFunction(ModeValues.SHA2, DigestSizes.d384);
                case KeyAgreementMacType.HmacSha2D512:
                    return new HashFunction(ModeValues.SHA2, DigestSizes.d512);
                default:
                    throw new ArgumentException($"invalid {nameof(parametersMacType)}");
            }
        }
    }
}
