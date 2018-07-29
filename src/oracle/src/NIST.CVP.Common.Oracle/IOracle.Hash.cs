using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        HashResult GetShaCase(ShaParameters param);
        HashResult GetSha3Case(Sha3Parameters param);

        MctResult<HashResult> GetShaMctCase(ShaParameters param);
        MctResult<HashResult> GetSha3MctCase(Sha3Parameters param);


        Task<HashResult> GetShaCaseAsync(ShaParameters param);
        Task<HashResult> GetSha3CaseAsync(Sha3Parameters param);

        Task<MctResult<HashResult>> GetShaMctCaseAsync(ShaParameters param);
        Task<MctResult<HashResult>> GetSha3MctCaseAsync(Sha3Parameters param);
    }
}
