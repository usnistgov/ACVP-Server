using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.CMAC;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.KC
{
    public class KeyConfirmationCmac : KeyConfirmationBase
    {
        private readonly ICmac _iCmac;

        public KeyConfirmationCmac(
            IKeyConfirmationMacDataCreator macDataCreator,
            IKeyConfirmationParameters keyConfirmationParameters,
            ICmac iCmac)
            : base(macDataCreator, keyConfirmationParameters)
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
