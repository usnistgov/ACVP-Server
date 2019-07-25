using System;
using System.Linq;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.MAC.CMAC;
using NIST.CVP.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.Crypto.Common.MAC.HMAC;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Symmetric.Engines;

namespace NIST.CVP.Crypto.KAS.KC
{
    public class KeyConfirmationFactory : IKeyConfirmationFactory
    {
        private readonly IKeyConfirmationMacDataCreator _macDataCreator;
        private readonly ICmacFactory _cmacFactory;
        private readonly IHmacFactory _hmacFactory;
        
        public KeyConfirmationFactory(IKeyConfirmationMacDataCreator macDataCreator)
        {
            _macDataCreator = macDataCreator;
            _cmacFactory = new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory());
            _hmacFactory = new HmacFactory(new ShaFactory());
        }

        public IKeyConfirmation GetInstance(IKeyConfirmationParameters parameters)
        {
            switch (parameters.MacType)
            {
                case KeyAgreementMacType.AesCcm:
                    ConfirmKeyLengthAesCcm(parameters.KeyLength);
                    return new KeyConfirmationAesCcm(
                        _macDataCreator, 
                        parameters,
                        new CcmBlockCipher(new AesEngine(), new ModeBlockCipherFactory(), new AES_CCMInternals()));
                case KeyAgreementMacType.CmacAes:
                    var cmacEnum = MapCmacEnum(parameters.KeyLength);

                    return new KeyConfirmationCmac(
                        _macDataCreator, parameters, _cmacFactory.GetCmacInstance(cmacEnum));
                case KeyAgreementMacType.HmacSha2D224:
                case KeyAgreementMacType.HmacSha2D256:
                case KeyAgreementMacType.HmacSha2D384:
                case KeyAgreementMacType.HmacSha2D512:
                    var hashFunction = GetHashFunction(parameters.MacType);

                    return new KeyConfirmationHmac(
                        _macDataCreator, parameters, _hmacFactory.GetHmacInstance(hashFunction));
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
