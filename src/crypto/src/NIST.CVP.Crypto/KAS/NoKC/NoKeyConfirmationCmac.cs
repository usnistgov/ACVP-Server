﻿using System;
using NIST.CVP.Crypto.Common.KAS.NoKC;
using NIST.CVP.Crypto.Common.MAC.CMAC;
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