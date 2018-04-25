using System;
using System.Text;
using NIST.CVP.Crypto.Common.KAS;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.NoKC
{
    public abstract class NoKeyConfirmationBase : INoKeyConfirmation
    {
        public const string STANDARD_MESSAGE = "Standard Test Message";
        protected readonly INoKeyConfirmationParameters NoKeyConfirmationParameters;

        protected NoKeyConfirmationBase(INoKeyConfirmationParameters noKeyConfirmationParameters)
        {
            NoKeyConfirmationParameters = noKeyConfirmationParameters;

            if (BitString.IsZeroLengthOrNull(NoKeyConfirmationParameters.DerivedKeyingMaterial))
            {
                throw new ArgumentException(nameof(NoKeyConfirmationParameters.DerivedKeyingMaterial));
            }
            if (BitString.IsZeroLengthOrNull(NoKeyConfirmationParameters.Nonce))
            {
                throw new ArgumentException(nameof(NoKeyConfirmationParameters.Nonce));
            }
        }

        public ComputeKeyMacResult ComputeMac()
        {
            var macData =
                new BitString(
                    Encoding.ASCII.GetBytes(STANDARD_MESSAGE)
                )
                .ConcatenateBits(NoKeyConfirmationParameters.Nonce);

            return new ComputeKeyMacResult(macData, Mac(macData));
        }

        protected abstract BitString Mac(BitString macData);
    }
}
