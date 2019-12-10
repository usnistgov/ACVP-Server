using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1;

namespace NIST.CVP.Orleans.Grains.Kas
{
    public interface IKasValTestGenerator<in TParameter, out TResult>
        where TParameter : KasValParametersBase
        where TResult : KasValResultBase
    {
        TResult GetTest(TParameter param);
    }
}