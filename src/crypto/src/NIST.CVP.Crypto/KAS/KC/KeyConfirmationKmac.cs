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
        private readonly IKmac _kmac;

        public KeyConfirmationKmac(
            IKeyConfirmationMacDataCreator macDataCreator, 
            IKeyConfirmationParameters keyConfirmationParameters,
            IKmac kmac) : 
            base(macDataCreator, keyConfirmationParameters)
        {
            _kmac = kmac;
        }

        protected override BitString Mac(BitString macData)
        {
            var result = _kmac.Generate(
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