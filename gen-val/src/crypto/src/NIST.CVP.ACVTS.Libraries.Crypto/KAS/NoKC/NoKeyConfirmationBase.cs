using System;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.NoKC;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.NoKC
{
    public abstract class NoKeyConfirmationBase : INoKeyConfirmation
    {
        protected readonly INoKeyConfirmationParameters NoKeyConfirmationParameters;
        private readonly INoKeyConfirmationMacDataCreator _macDataCreator;

        protected NoKeyConfirmationBase(
            INoKeyConfirmationMacDataCreator macDataCreator,
            INoKeyConfirmationParameters noKeyConfirmationParameters)
        {
            _macDataCreator = macDataCreator;
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
            var macData = _macDataCreator.GetMacData(NoKeyConfirmationParameters);

            return new ComputeKeyMacResult(macData, Mac(macData));
        }

        protected abstract BitString Mac(BitString macData);
    }
}
