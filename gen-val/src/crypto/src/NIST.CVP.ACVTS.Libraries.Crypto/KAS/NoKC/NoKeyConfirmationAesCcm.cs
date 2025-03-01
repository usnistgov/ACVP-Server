﻿using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.NoKC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.BlockModes.Aead;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.Symmetric.Enums;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.NoKC
{
    public class NoKeyConfirmationAesCcm : NoKeyConfirmationBase
    {
        private readonly IAeadModeBlockCipher _algo;

        public NoKeyConfirmationAesCcm(INoKeyConfirmationMacDataCreator macDataCreator,
            INoKeyConfirmationParameters noKeyConfirmationParameters,
            IAeadModeBlockCipher algo)
            : base(macDataCreator, noKeyConfirmationParameters)
        {
            _algo = algo;

            if (BitString.IsZeroLengthOrNull(noKeyConfirmationParameters.CcmNonce))
            {
                throw new ArgumentException(nameof(noKeyConfirmationParameters.CcmNonce));
            }
        }

        protected override BitString Mac(BitString macData)
        {
            var param = new AeadModeBlockCipherParameters(
                BlockCipherDirections.Encrypt,
                NoKeyConfirmationParameters.CcmNonce,
                NoKeyConfirmationParameters.DerivedKeyingMaterial,
                new BitString(0),
                macData,
                NoKeyConfirmationParameters.MacLength
            );

            var mac = _algo.ProcessPayload(param);

            if (!mac.Success)
            {
                throw new Exception(mac.ErrorMessage);
            }

            return mac.Result;
        }
    }
}
