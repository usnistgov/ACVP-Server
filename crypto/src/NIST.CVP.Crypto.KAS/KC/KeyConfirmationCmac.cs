using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KC
{
    public class KeyConfirmationCmac : KeyConfirmationBase
    {
        private readonly ICmac _iCmac;
        private readonly KeyConfirmationParameters _keyConfirmationParameters;

        public KeyConfirmationCmac(ICmac iCmac, KeyConfirmationParameters keyConfirmationParameters)
        {
            _iCmac = iCmac;
            _keyConfirmationParameters = keyConfirmationParameters;
        }

        protected override BitString Mac(BitString macData)
        {
            var result = _iCmac.Generate(
                _keyConfirmationParameters.DerivedKeyingMaterial,
                macData,
                _keyConfirmationParameters.MacLength
            );

            if (!result.Success)
            {
                throw new Exception(result.ErrorMessage);
            }

            return result.ResultingMac;
        }
    }
}
