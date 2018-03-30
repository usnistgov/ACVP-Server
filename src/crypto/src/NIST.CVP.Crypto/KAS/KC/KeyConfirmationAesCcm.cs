using System;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.Symmetric.AES;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KC
{
    public class KeyConfirmationAesCcm : KeyConfirmationBase
    {
        private readonly IAES_CCM _iAesCcm;

        public KeyConfirmationAesCcm(IAES_CCM iAesCcm, IKeyConfirmationParameters keyConfirmationParameters)
        {
            _keyConfirmationParameters = keyConfirmationParameters;
            _iAesCcm = iAesCcm;
        }

        protected override BitString Mac(BitString macData)
        {
            var result = _iAesCcm.Encrypt(
                _keyConfirmationParameters.DerivedKeyingMaterial,
                _keyConfirmationParameters.CcmNonce,
                new BitString(0),
                macData,
                _keyConfirmationParameters.MacLength
            );

            if (!result.Success)
            {
                throw new Exception(result.ErrorMessage);
            }

            return result.Result;
        }
    }
}
