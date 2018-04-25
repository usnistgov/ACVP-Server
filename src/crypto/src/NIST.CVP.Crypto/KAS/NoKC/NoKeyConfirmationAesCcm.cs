using System;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.Symmetric.AES;
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

            return mac.Result;
        }
    }
}
