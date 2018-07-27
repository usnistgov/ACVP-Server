using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;

namespace NIST.CVP.Common.Oracle
{
    public partial interface IOracle
    {
        KeyWrapResult GetKeyWrapCase(KeyWrapParameters param);


        Task<KeyWrapResult> GetKeyWrapCaseAsync(KeyWrapParameters param);
    }
}
