using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Orleans.Grains.Kas
{
    public interface IKasAftTestGenerator<in TKasAftParameters, out TKasAftResult>
        where TKasAftParameters : KasAftParametersBase
        where TKasAftResult : KasAftResultBase
    {
        TKasAftResult GetTest(TKasAftParameters param);
    }
}