using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Threading.Tasks;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        Task<HashResult> GetShaCaseAsync(ShaParameters param);
        Task<HashResult> GetSha3CaseAsync(Sha3Parameters param);
        Task<CShakeResult> GetCShakeCaseAsync(CShakeParameters param);
        Task<ParallelHashResult> GetParallelHashCaseAsync(ParallelHashParameters param);
        Task<TupleHashResult> GetTupleHashCaseAsync(TupleHashParameters param);

        Task<MctResult<HashResult>> GetShaMctCaseAsync(ShaParameters param);
        Task<MctResult<HashResult>> GetSha3MctCaseAsync(Sha3Parameters param);
        Task<MctResult<HashResult>> GetShakeMctCaseAsync(ShakeParameters param);
        Task<MctResult<CShakeResult>> GetCShakeMctCaseAsync(CShakeParameters param);
        Task<MctResult<ParallelHashResult>> GetParallelHashMctCaseAsync(ParallelHashParameters param);
        Task<MctResult<TupleHashResult>> GetTupleHashMctCaseAsync(TupleHashParameters param);
    }
}
