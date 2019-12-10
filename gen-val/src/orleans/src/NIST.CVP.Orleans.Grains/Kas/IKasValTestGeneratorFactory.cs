using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ParameterTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Common.Oracle.ResultTypes.Kas.Sp800_56Ar1;
using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Orleans.Grains.Kas
{
    public interface IKasValTestGeneratorFactory<in TKasValParameters, out TKasValResult>
        where TKasValParameters : KasValParametersBase
        where TKasValResult : KasValResultBase
    {
        IKasValTestGenerator<TKasValParameters, TKasValResult> GetInstance(KasMode kasMode);
    }
}