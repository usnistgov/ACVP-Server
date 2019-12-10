using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1;

namespace NIST.CVP.Orleans.Grains.Kas
{
    public interface IKasAftTestGenerator<in TKasAftParameters, out TKasAftResult>
        where TKasAftParameters : KasAftParametersBase
        where TKasAftResult : KasAftResultBase
    {
        TKasAftResult GetTest(TKasAftParameters param);
    }
}