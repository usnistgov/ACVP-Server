using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        HashResult GetSha1Case();
        HashResult GetSha2Case();
        HashResult GetSha3Case();
        HashResult GetShakeCase();

        MctResult<HashResult> GetSha1MctCase();
        MctResult<HashResult> GetSha2MctCase();
        MctResult<HashResult> GetSha3MctCase();
        MctResult<HashResult> GetShakeMctCase();

        HashResult GetShakeVotCase();
    }
}
