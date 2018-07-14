using System;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Oracle.KAS.Ecc;
using NIST.CVP.Crypto.Oracle.KAS.Ffc;

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
            // TODO utilize oracle calls to DSA functions

            return new KasValFfcTestGeneratorFactory()
                .GetInstance(param.KasMode)
                .GetTest(param);
        }

        public KasAftResultFfc GetKasAftTestFfc(KasAftParametersFfc param)
        {
            // TODO utilize oracle calls to DSA functions

            return new KasAftFfcTestGeneratorFactory()
                .GetInstance(param.KasMode)
                .GetTest(param);
        }

        public KasAftDeferredResult CompleteDeferredKasTest(KasAftDeferredParametersFfc param)
        {
            return new KasAftFfcDeferredTestResolverFactory()
                .GetInstance(param.KasMode)
                .CompleteTest(param);
        }

        public KasEccComponentResult GetKasEccComponentTest(KasEccComponentParameters param)
        {
            throw new NotImplementedException();
        }

        public KasEccComponentResult CompleteDeferredKasComponentTest(KasEccComponentDeferredParameters param)
        {
            throw new NotImplementedException();
        }
    }
}
