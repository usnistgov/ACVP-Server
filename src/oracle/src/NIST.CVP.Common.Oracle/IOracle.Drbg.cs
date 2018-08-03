using System.Threading.Tasks;
using NIST.CVP.Crypto.Common.DRBG;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        Task<ResultTypes.DrbgResult> GetDrbgCaseAsync(DrbgParameters param);
    }
}
