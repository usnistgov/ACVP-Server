using System;
using System.Linq;
using NIST.CVP.ACVTS.Libraries.Crypto.AES_CCM;
using NIST.CVP.ACVTS.Libraries.Crypto.CMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Helpers;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.CMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.KMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.CSHAKE;
using NIST.CVP.ACVTS.Libraries.Crypto.HMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.KMAC;
using NIST.CVP.ACVTS.Libraries.Crypto.SHA.NativeFastSha;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.BlockModes.Aead;
using NIST.CVP.ACVTS.Libraries.Crypto.Symmetric.Engines;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.KC
{
    public class KeyConfirmationFactory : IKeyConfirmationFactory
    {
        private readonly IKeyConfirmationMacDataCreator _macDataCreator;
        private readonly ICmacFactory _cmacFactory;
        private readonly IHmacFactory _hmacFactory;
        private readonly IKmacFactory _kmacFactory;

        public KeyConfirmationFactory(IKeyConfirmationMacDataCreator macDataCreator)
        {
            _macDataCreator = macDataCreator;
            _cmacFactory = new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory());
            _hmacFactory = new HmacFactory(new NativeShaFactory());
            _kmacFactory = new KmacFactory(new CSHAKEWrapper());
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
                case KeyAgreementMacType.HmacSha1:
                case KeyAgreementMacType.HmacSha2D224:
                case KeyAgreementMacType.HmacSha2D256:
                case KeyAgreementMacType.HmacSha2D384:
                case KeyAgreementMacType.HmacSha2D512:
                case KeyAgreementMacType.HmacSha2D512_T224:
                case KeyAgreementMacType.HmacSha2D512_T256:
                case KeyAgreementMacType.HmacSha3D224:
                case KeyAgreementMacType.HmacSha3D256:
                case KeyAgreementMacType.HmacSha3D384:
                case KeyAgreementMacType.HmacSha3D512:
                    ModeValues modeValue = ModeValues.SHA2;
                    DigestSizes digestSize = DigestSizes.NONE;
                    EnumMapping.GetHashFunctionOptions(parameters.MacType, ref modeValue, ref digestSize);

                    return new KeyConfirmationHmac(
                        _macDataCreator, parameters, _hmacFactory.GetHmacInstance(new HashFunction(modeValue, digestSize)));
                case KeyAgreementMacType.Kmac_128:
                    return new KeyConfirmationKmac(_macDataCreator, parameters, _kmacFactory, 256);
                case KeyAgreementMacType.Kmac_256:
                    return new KeyConfirmationKmac(_macDataCreator, parameters, _kmacFactory, 512);
                default:
                    throw new ArgumentException($"{GetType().Name}, {nameof(parameters.MacType)}");
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
    }
}
