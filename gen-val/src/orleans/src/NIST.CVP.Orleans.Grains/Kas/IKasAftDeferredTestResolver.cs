using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Orleans.Grains.Kas
{
    public interface IKasAftDeferredTestResolver<in TKasAftDeferredParameters, out TKasAftDeferredResult>
        where TKasAftDeferredParameters : KasAftDeferredParametersBase
        where TKasAftDeferredResult : KasAftDeferredResult
    {
        TKasAftDeferredResult CompleteTest(TKasAftDeferredParameters param);
    }
}