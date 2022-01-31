using NIST.CVP.ACVTS.Libraries.Crypto.Common.KAS.Enums;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar1;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Ar1
{
    public interface IKasAftDeferredTestResolverFactory<in TKasAftDeferredParameters, out TKasAftDeferredResult>
        where TKasAftDeferredParameters : KasAftDeferredParametersBase
        where TKasAftDeferredResult : KasAftDeferredResult
    {
        IKasAftDeferredTestResolver<TKasAftDeferredParameters, TKasAftDeferredResult> GetInstance(KasMode kasMode);
    }
}
