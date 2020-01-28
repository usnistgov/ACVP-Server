using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Orleans.Grains.Kas.Sp800_56Ar1
{
    public interface IKasAftTestGeneratorFactory<in TKasAftParameters, out TKasAftResult>
        where TKasAftParameters : KasAftParametersBase
        where TKasAftResult : KasAftResultBase
    {
        IKasAftTestGenerator<TKasAftParameters, TKasAftResult> GetInstance(KasMode kasMode);
    }
}