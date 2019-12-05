using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Orleans.Grains.Kas
{
    public interface IKasAftDeferredTestResolverFactory<in TKasAftDeferredParameters, out TKasAftDeferredResult>
        where TKasAftDeferredParameters : KasAftDeferredParametersBase
        where TKasAftDeferredResult : KasAftDeferredResult
    {
        IKasAftDeferredTestResolver<TKasAftDeferredParameters, TKasAftDeferredResult> GetInstance(KasMode kasMode);
    }
}