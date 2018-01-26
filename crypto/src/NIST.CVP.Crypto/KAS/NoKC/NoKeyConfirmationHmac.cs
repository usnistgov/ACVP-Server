using System;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.MAC.HMAC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.NoKC
{
    public class NoKeyConfirmationHmac : NoKeyConfirmationBase
    {
        private readonly IHmac Algo;

        public NoKeyConfirmationHmac(INoKeyConfirmationParameters noKeyConfirmationParameters, IHmac algo) 
            : base(noKeyConfirmationParameters)
        {
            Algo = algo;
        }

        protected override BitString Mac(BitString macData)
        {
            var mac = Algo.Generate(
                NoKeyConfirmationParameters.DerivedKeyingMaterial, 
                macData,
                NoKeyConfirmationParameters.MacLength
            );

            if (!mac.Success)
            {
                throw new Exception(mac.ErrorMessage);
            }

            return mac.Mac;
        }
    }
}
