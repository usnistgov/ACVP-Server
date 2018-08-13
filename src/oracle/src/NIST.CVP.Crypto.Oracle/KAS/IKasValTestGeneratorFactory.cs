using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Crypto.Common.KAS.Enums;

namespace NIST.CVP.Crypto.Oracle.KAS
{
    public interface IKasValTestGeneratorFactory<in TKasValParameters, out TKasValResult>
        where TKasValParameters : KasValParametersBase
        where TKasValResult : KasValResultBase
    {
        IKasValTestGenerator<TKasValParameters, TKasValResult> GetInstance(KasMode kasMode);
    }
}