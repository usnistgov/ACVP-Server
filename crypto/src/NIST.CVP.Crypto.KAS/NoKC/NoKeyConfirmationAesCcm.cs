using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.AES_CCM;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.NoKC
{
    public class NoKeyConfirmationAesCcm : NoKeyConfirmationBase
    {
        private readonly IAES_CCM Algo;

        public NoKeyConfirmationAesCcm(INoKeyConfirmationParameters noKeyConfirmationParameters, IAES_CCM algo) 
            : base(noKeyConfirmationParameters)
        {
            Algo = algo;

            if (BitString.IsZeroLengthOrNull(noKeyConfirmationParameters.CcmNonce))
            {
                throw new ArgumentException(nameof(noKeyConfirmationParameters.CcmNonce));
            }
        }

        protected override BitString Mac(BitString macData)
        {
            var mac = Algo.Encrypt(
                NoKeyConfirmationParameters.DerivedKeyingMaterial, 
                NoKeyConfirmationParameters.CcmNonce,
                new BitString(0), 
                macData, 
                NoKeyConfirmationParameters.MacLength
            );

            if (!mac.Success)
            {
                throw new Exception(mac.ErrorMessage);
            }

            return mac.CipherText;
        }
    }
}
