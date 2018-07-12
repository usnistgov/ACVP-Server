using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Oracle.KAS.Ecc;

namespace NIST.CVP.Crypto.Oracle
{
    public partial class Oracle
    {
        public KasValResultEcc GetKasValTestEcc(KasValParametersEcc param)
        {
            // TODO utilize oracle calls to ECDSA functions

            return new KasValEccTestGeneratorFactory()
                .GetInstance(param.KasMode)
                .GetTest(param);
        }

        public KasAftResultEcc GetKasAftTestEcc(KasAftParametersEcc param)
        {
            // TODO utilize oracle calls to ECDSA functions

            return new KasAftEccTestGeneratorFactory()
                .GetInstance(param.KasMode)
                .GetTest(param);
        }

        public KasAftDeferredResult CompleteDeferredKasTest(KasAftDeferredParametersEcc param)
        {
            return new KasAftEccDeferredTestResolverFactory()
                .GetInstance(param.KasMode)
                .CompleteTest(param);
        }

        public KasValResultFfc GetKasValTestFfc(KasValParametersFfc param)
        {
            throw new NotImplementedException();
        }

        public KasAftResultFfc GetKasAftTestFfc(KasAftParametersFfc param)
        {
            throw new NotImplementedException();
        }

        public KasAftDeferredResult CompleteDeferredKasTest(KasAftDeferredParametersFfc param)
        {
            throw new NotImplementedException();
        }
    }
}
