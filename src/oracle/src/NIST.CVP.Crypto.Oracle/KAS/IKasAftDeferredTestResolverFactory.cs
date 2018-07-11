using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Oracle.KAS
{
    public interface IKasAftDeferredTestResolverFactory<in TKasAftDeferredParameters, out TKasAftDeferredResult>
        where TKasAftDeferredParameters : KasAftDeferredParametersBase
        where TKasAftDeferredResult : KasAftDeferredResult
    {
        IKasAftDeferredTestResolver<TKasAftDeferredParameters, TKasAftDeferredResult> GetInstance(KasMode kasMode);
    }
}