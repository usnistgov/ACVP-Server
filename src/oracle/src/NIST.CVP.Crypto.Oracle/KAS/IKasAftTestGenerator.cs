using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Crypto.Oracle.KAS
{
    public interface IKasAftTestGenerator<in TKasAftParameters, out TKasAftResult>
        where TKasAftParameters : KasAftParametersBase
        where TKasAftResult : KasAftResultBase
    {
        TKasAftResult GetTest(TKasAftParameters param);
    }
}