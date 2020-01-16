using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Orleans.Grains.Kas.Sp800_56Ar1
{
    public interface IKasAftDeferredTestResolverFactory<in TKasAftDeferredParameters, out TKasAftDeferredResult>
        where TKasAftDeferredParameters : KasAftDeferredParametersBase
        where TKasAftDeferredResult : KasAftDeferredResult
    {
        IKasAftDeferredTestResolver<TKasAftDeferredParameters, TKasAftDeferredResult> GetInstance(KasMode kasMode);
    }
}