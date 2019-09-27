using System;
using System.Text;
using NIST.CVP.Crypto.Common.KAS.KC;
using NIST.CVP.Crypto.Common.MAC.KMAC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.KC
{
    public class KeyConfirmationKmac : KeyConfirmationBase
    {
        private static readonly BitString Customization = new BitString(Encoding.ASCII.GetBytes("KC"));
        private readonly IKmacFactory _kmacFactory;
        private readonly int _capacity;

        public KeyConfirmationKmac(
            IKeyConfirmationMacDataCreator macDataCreator, 
            IKeyConfirmationParameters keyConfirmationParameters,
            IKmacFactory kmacFactory, int capacity) : 
            base(macDataCreator, keyConfirmationParameters)
        {
            _kmacFactory = kmacFactory;
            _capacity = capacity;
        }

        protected override BitString Mac(BitString macData)
        {
            var result = _kmacFactory.GetKmacInstance(_capacity, false).Generate(
                _keyConfirmationParameters.DerivedKeyingMaterial,
                macData,
                Customization,
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