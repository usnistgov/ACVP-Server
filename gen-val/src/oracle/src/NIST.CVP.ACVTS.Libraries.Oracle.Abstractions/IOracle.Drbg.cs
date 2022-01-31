using System.Threading.Tasks;
using NIST.CVP.ACVTS.Libraries.Crypto.Common.DRBG;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ParameterTypes;
using NIST.CVP.ACVTS.Libraries.Oracle.Abstractions.ResultTypes;

namespace NIST.CVP.ACVTS.Libraries.Oracle.Abstractions
{
    public partial interface IOracle
    {
        Task<ResultTypes.DrbgResult> GetDrbgCaseAsync(DrbgParameters param);
        Task<HashResult> GetHashDfCaseAsync(ShaWrapperParameters param);
        Task<AesResult> GetBlockCipherDfCaseAsync(AesParameters param);
    }
}
