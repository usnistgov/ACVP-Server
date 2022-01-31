using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes.Kas.Sp800_56Ar1;

namespace NIST.CVP.ACVTS.Libraries.Orleans.Grains.Kas.Sp800_56Ar1
{
    public interface IKasValTestGenerator<in TParameter, out TResult>
        where TParameter : KasValParametersBase
        where TResult : KasValResultBase
    {
        TResult GetTest(TParameter param);
    }
}
