using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Models;

namespace NIST.CVP.Pools.PoolModels
{
    public class AesPool : PoolBase<AesParameters, AesResult>
    {
        public AesPool(PoolConstructionParameters<AesParameters> param)
            : base(param) { }
    }
}
