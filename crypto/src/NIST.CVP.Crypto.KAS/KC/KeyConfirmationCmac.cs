using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KC
{
    public class KeyConfirmationCmac : KeyConfirmationBase<KeyConfirmationParameters>
    {
        private readonly ICmac _iCmac;

        public KeyConfirmationCmac(ICmac iCmac)
        {
            _iCmac = iCmac;
        }

        protected override BitString Mac(KeyConfirmationParameters keyConfirmationParameters, BitString macData)
        {
            var result = _iCmac.Generate(
                keyConfirmationParameters.DerivedKeyingMaterial,
                macData,
                keyConfirmationParameters.MacLength
            );

            if (!result.Success)
            {
                throw new Exception(result.ErrorMessage);
            }

            return result.ResultingMac;
        }
    }
}
