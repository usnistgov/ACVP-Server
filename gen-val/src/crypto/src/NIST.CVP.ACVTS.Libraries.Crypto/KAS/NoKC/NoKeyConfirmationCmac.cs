using System;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.NoKC;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.MAC.CMAC;
using NIST.CVP.ACVTS.Libraries.Math;

namespace NIST.CVP.ACVTS.Libraries.Crypto.KAS.NoKC
{
    public class NoKeyConfirmationCmac : NoKeyConfirmationBase
    {
        private readonly ICmac Algo;

        public NoKeyConfirmationCmac(
            INoKeyConfirmationMacDataCreator macDataCreator,
            INoKeyConfirmationParameters noKeyConfirmationParameters,
            ICmac algo)
            : base(macDataCreator, noKeyConfirmationParameters)
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
