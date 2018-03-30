using System;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.MAC.CMAC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KC
{
    public class KeyConfirmationCmac : KeyConfirmationBase
    {
        private readonly ICmac _iCmac;

        public KeyConfirmationCmac(ICmac iCmac, IKeyConfirmationParameters keyConfirmationParameters)
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

            return result.Mac;
        }
    }
}
