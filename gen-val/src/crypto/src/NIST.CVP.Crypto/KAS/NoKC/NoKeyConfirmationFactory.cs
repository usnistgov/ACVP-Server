using System;
using NIST.CVP.Crypto.AES;
using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Crypto.Common.Hash.ShaWrapper;
using NIST.CVP.Crypto.Common.KAS.Enums;
using NIST.CVP.Crypto.Common.KAS.Helpers;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.MAC.CMAC;
using NIST.CVP.Crypto.Common.MAC.CMAC.Enums;
using NIST.CVP.Crypto.Common.MAC.HMAC;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Crypto.SHAWrapper;
using NIST.CVP.Crypto.Symmetric.BlockModes;
using NIST.CVP.Crypto.Symmetric.BlockModes.Aead;
using NIST.CVP.Crypto.Symmetric.Engines;

namespace NIST.CVP.Crypto.KAS.NoKC
{
    public class NoKeyConfirmationFactory : INoKeyConfirmationFactory
    {
        private readonly INoKeyConfirmationMacDataCreator _macDataCreator;
        private readonly ICmacFactory _cmacFactory;
        private readonly IHmacFactory _hmacFactory;

        public NoKeyConfirmationFactory(INoKeyConfirmationMacDataCreator macDataCreator)
        {
            _macDataCreator = macDataCreator;
            _cmacFactory = new CmacFactory(new BlockCipherEngineFactory(), new ModeBlockCipherFactory());
            _hmacFactory = new HmacFactory(new ShaFactory());
        }
        
        public INoKeyConfirmation GetInstance(INoKeyConfirmationParameters parameters)
        {
            switch (parameters.KeyAgreementMacType)
            {
                case KeyAgreementMacType.AesCcm:
                    return new NoKeyConfirmationAesCcm(_macDataCreator, parameters, new CcmBlockCipher(new AesEngine(), new ModeBlockCipherFactory(), new AES_CCMInternals()));
                case KeyAgreementMacType.CmacAes:
                    return new NoKeyConfirmationCmac(_macDataCreator, parameters, _cmacFactory.GetCmacInstance(CmacTypes.AES128)); // doesn't matter as long as aea
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
                    EnumMapping.GetHashFunctionOptions(parameters.KeyAgreementMacType, ref modeValue, ref digestSize);
                    return new NoKeyConfirmationHmac(_macDataCreator, parameters, _hmacFactory.GetHmacInstance(new HashFunction(modeValue, digestSize)));
                default:
                     throw new ArgumentException($"{GetType().Name}, {nameof(parameters.KeyAgreementMacType)}");
            }
        }
    }
}
