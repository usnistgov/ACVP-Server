using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Oracle.KAS
{
    public interface IKasAftTestGeneratorFactory<in TKasAftParameters, out TKasAftResult>
        where TKasAftParameters : KasAftParametersBase
        where TKasAftResult : KasAftResultBase
    {
        IKasAftTestGenerator<TKasAftParameters, TKasAftResult> GetInstance(KasMode kasMode);
    }
}