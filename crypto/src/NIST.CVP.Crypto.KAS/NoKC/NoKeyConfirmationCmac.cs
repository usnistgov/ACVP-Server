using System;
using System.Collections.Generic;
using System.Text;
using NIST.CVP.Crypto.CMAC;
using NIST.CVP.Math;

namespace NIST.CVP.Crypto.KAS.NoKC
{
    public class NoKeyConfirmationCmac : NoKeyConfirmationBase
    {
        private readonly ICmac Algo;

        public NoKeyConfirmationCmac(INoKeyConfirmationParameters noKeyConfirmationParameters, ICmac algo) 
            : base(noKeyConfirmationParameters)
        {
            Algo = algo;
        }

        protected override BitString Mac(BitString macData)
        {
            var mac =  Algo.Generate(
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
