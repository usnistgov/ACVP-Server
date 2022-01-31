using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions
{
    public partial interface IOracle
    {
        Task<MacResult> GetCmacCaseAsync(CmacParameters param);
        Task<MacResult> GetHmacCaseAsync(HmacParameters param);
        Task<KmacResult> GetKmacCaseAsync(KmacParameters param);
    }
}
