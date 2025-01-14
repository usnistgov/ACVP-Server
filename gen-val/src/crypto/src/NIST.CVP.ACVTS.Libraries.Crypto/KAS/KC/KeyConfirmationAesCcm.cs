﻿using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.AES;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.KC
{
    public class KeyConfirmationAesCcm : KeyConfirmationBase
    {
        private readonly IAeadModeBlockCipher _ccm;

        public KeyConfirmationAesCcm(
            IKeyConfirmationMacDataCreator macDataCreator,
            IKeyConfirmationParameters keyConfirmationParameters,
            IAeadModeBlockCipher ccm
            )
            : base(macDataCreator, keyConfirmationParameters)
        {
            _ccm = ccm;
            _keyConfirmationParameters = keyConfirmationParameters;
        }

        protected override BitString Mac(BitString macData)
        {
            var param = new AeadModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                _keyConfirmationParameters.CcmNonce,
                _keyConfirmationParameters.DerivedKeyingMaterial,
                new BitString(0),
                macData,
                _keyConfirmationParameters.MacLength
            );

            var result = _ccm.ProcessPayload(param);

            if (!result.Success)
            {
                throw new Exception(result.ErrorMessage);
            }

            return result.Result;
        }
    }
}
