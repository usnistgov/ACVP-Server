﻿using System;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.MAC.HMAC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KC
{
    public class KeyConfirmationHmac : KeyConfirmationBase
    {
        private readonly IHmac _iHmac;

        public KeyConfirmationHmac(
            IKeyConfirmationMacDataCreator macDataCreator, 
            IKeyConfirmationParameters keyConfirmationParameters,
            IHmac iHmac)
            : base (macDataCreator, keyConfirmationParameters)
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