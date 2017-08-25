using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.HMAC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KC
{
    public class KeyConfirmationHmac : KeyConfirmationBase<KeyConfirmationParameters>
    {
        private readonly IHmac _iHmac;

        public KeyConfirmationHmac(IHmac iHmac, KeyConfirmationParameters keyConfirmationParameters)
        {
            _iHmac = iHmac;
            _keyConfirmationParameters = keyConfirmationParameters;
        }

        protected override BitString Mac(BitString macData)
        {
            var result = _iHmac.Generate(
                _keyConfirmationParameters.DerivedKeyingMaterial,
                macData,
                _keyConfirmationParameters.MacLength
            );

            if (!result.Success)
            {
                throw new Exception(result.ErrorMessage);
            }

            return result.Mac;
        }
    }
}
