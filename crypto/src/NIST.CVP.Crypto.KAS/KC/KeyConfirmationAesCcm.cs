using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KC
{
    public class KeyConfirmationAesCcm : KeyConfirmationBase
    {
        private const string _STANDARD_MESSAGE = "Standard Test Message";
        private readonly KeyConfirmationParametersAesCcm _keyConfirmationParameters;
        private readonly IAES_CCM _iAesCcm;

        public KeyConfirmationAesCcm(IAES_CCM iAesCcm, KeyConfirmationParametersAesCcm keyConfirmationParameters)
        {
            _keyConfirmationParameters = keyConfirmationParameters;
            _iAesCcm = iAesCcm;
        }

        protected override BitString Mac(BitString macData)
        {
            var result = _iAesCcm.Encrypt(
                _keyConfirmationParameters.DerivedKeyingMaterial,
                _keyConfirmationParameters.Nonce,
                new BitString(0), 
                macData,
                _keyConfirmationParameters.MacLength
            );

            if (!result.Success)
            {
                throw new Exception(result.ErrorMessage);
            }

            return result.CipherText;
        }
    }
}
