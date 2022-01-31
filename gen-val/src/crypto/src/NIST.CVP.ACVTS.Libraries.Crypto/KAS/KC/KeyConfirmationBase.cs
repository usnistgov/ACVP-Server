using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.ACVTS.Libraries.Common.ExtensionMethods;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.KC;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.KC
{
    /// <inheritdoc />
    public abstract class KeyConfirmationBase : IKeyConfirmation
    {
        protected IKeyConfirmationParameters _keyConfirmationParameters;
        protected readonly IKeyConfirmationMacDataCreator _macDataCreator;

        protected KeyConfirmationBase(IKeyConfirmationMacDataCreator macDataCreator, IKeyConfirmationParameters keyConfirmationParameters)
        {
            _macDataCreator = macDataCreator;
            _keyConfirmationParameters = keyConfirmationParameters;
        }

        public ComputeKeyMacResult ComputeMac()
        {
            var macData = _macDataCreator.GetMacData(_keyConfirmationParameters);

            var result = Mac(macData);

            return new ComputeKeyMacResult(macData, result);
        }

        protected abstract BitString Mac(BitString macData);
    }
}
