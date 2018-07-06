using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        HashResult GetShaCase(HashParameters param);
        HashResult GetSha3Case();
        HashResult GetShakeCase();

        MctResult<HashResult> GetShaMctCase(HashParameters param);
        MctResult<HashResult> GetSha3MctCase();
        MctResult<HashResult> GetShakeMctCase();

        HashResult GetShakeVotCase();
    }
}
