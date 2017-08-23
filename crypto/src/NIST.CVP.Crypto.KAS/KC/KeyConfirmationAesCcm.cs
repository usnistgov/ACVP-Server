using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KC
{
    public class KeyConfirmationAesCcm : KeyConfirmationBase<KeyConfirmationParametersAesCcm>
    {
        private const string _STANDARD_MESSAGE = "Standard Test Message";

        private readonly IAES_CCM _iAesCcm;

        public KeyConfirmationAesCcm(IAES_CCM iAesCcm)
        {
            _iAesCcm = iAesCcm;
        }

        protected override BitString Mac(KeyConfirmationParametersAesCcm keyConfirmationParameters, BitString macData)
        {
            var result = _iAesCcm.Encrypt(
                keyConfirmationParameters.DerivedKeyingMaterial,
                keyConfirmationParameters.CcmNonce,
                new BitString(0), 
                macData,
                keyConfirmationParameters.MacLength
            );

            if (!result.Success)
            {
                throw new Exception(result.ErrorMessage);
            }

            return result.CipherText;
        }
    }
}
