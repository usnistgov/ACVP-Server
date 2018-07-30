using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        HashResult GetShaCase(ShaParameters param);
        HashResult GetSha3Case(Sha3Parameters param);
        HashResultCSHAKE GetCShakeCase(CSHAKEParameters param);
        HashResultParallelHash GetParallelHashCase(ParallelHashParameters param);

        MctResult<HashResult> GetShaMctCase(ShaParameters param);
        MctResult<HashResult> GetSha3MctCase(Sha3Parameters param);
        MctResult<HashResultCSHAKE> GetCShakeMctCase(CSHAKEParameters param);
        MctResult<HashResultParallelHash> GetParallelHashMctCase(ParallelHashParameters param);
    }
}
