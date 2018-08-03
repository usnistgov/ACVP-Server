using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        HashResult GetShaCase(ShaParameters param);
        HashResult GetSha3Case(Sha3Parameters param);
        CShakeResult GetCShakeCase(CShakeParameters param);
        ParallelHashResult GetParallelHashCase(ParallelHashParameters param);
        TupleHashResult GetTupleHashCase(TupleHashParameters param);

        MctResult<HashResult> GetShaMctCase(ShaParameters param);
        MctResult<HashResult> GetSha3MctCase(Sha3Parameters param);
        MctResult<CShakeResult> GetCShakeMctCase(CShakeParameters param);
        MctResult<ParallelHashResult> GetParallelHashMctCase(ParallelHashParameters param);
        MctResult<TupleHashResult> GetTupleHashMctCase(TupleHashParameters param);


        Task<HashResult> GetShaCaseAsync(ShaParameters param);
        Task<HashResult> GetSha3CaseAsync(Sha3Parameters param);
        Task<CShakeResult> GetCShakeCaseAsync(CShakeParameters param);
        Task<ParallelHashResult> GetParallelHashCaseAsync(ParallelHashParameters param);
        Task<TupleHashResult> GetTupleHashCaseAsync(TupleHashParameters param);

        Task<MctResult<HashResult>> GetShaMctCaseAsync(ShaParameters param);
        Task<MctResult<HashResult>> GetSha3MctCaseAsync(Sha3Parameters param);
        Task<MctResult<CShakeResult>> GetCShakeMctCaseAsync(CShakeParameters param);
        Task<MctResult<ParallelHashResult>> GetParallelHashMctCaseAsync(ParallelHashParameters param);
        Task<MctResult<TupleHashResult>> GetTupleHashMctCaseAsync(TupleHashParameters param);
    }
}
