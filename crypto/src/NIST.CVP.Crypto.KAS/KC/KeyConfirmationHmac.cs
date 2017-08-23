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

        public KeyConfirmationHmac(IHmac iHmac)
        {
            _iHmac = iHmac;
        }

        protected override BitString Mac(KeyConfirmationParameters keyConfirmationParameters, BitString macData)
        {
            var result = _iHmac.Generate(
                keyConfirmationParameters.DerivedKeyingMaterial,
                macData,
                keyConfirmationParameters.MacLength
            );

            if (!result.Success)
            {
                throw new Exception(result.ErrorMessage);
            }

            return result.Mac;
        }
    }
}
