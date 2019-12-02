using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Orleans.Grains.Kas
{
    public interface IKasValTestGenerator<in TParameter, out TResult>
        where TParameter : KasValParametersBase
        where TResult : KasValResultBase
    {
        TResult GetTest(TParameter param);
    }
}