using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions
{
    public partial interface IOracle
    {
        Task<CShakeResult> GetCShakeCaseAsync(CShakeParameters param);
        Task<ParallelHashResult> GetParallelHashCaseAsync(ParallelHashParameters param);
        Task<TupleHashResult> GetTupleHashCaseAsync(TupleHashParameters param);
        
        Task<HashResult> GetShaCaseAsync(ShaParameters param);
        Task<HashResult> GetSha3CaseAsync(ShaParameters param);
        Task<MctResult<HashResult>> GetShaMctCaseAsync(ShaParameters param);
        Task<MctResult<HashResult>> GetSha3MctCaseAsync(ShaParameters param);
        
        Task<MctResult<HashResult>> GetShakeMctCaseAsync(ShakeParameters param);
        Task<MctResult<CShakeResult>> GetCShakeMctCaseAsync(CShakeParameters param);
        Task<MctResult<ParallelHashResult>> GetParallelHashMctCaseAsync(ParallelHashParameters param);
        Task<MctResult<TupleHashResult>> GetTupleHashMctCaseAsync(TupleHashParameters param);

        Task<LargeDataHashResult> GetShaLdtCaseAsync(ShaLargeDataParameters param);
    }
}
