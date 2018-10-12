using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Models;

namespace NIST.CVP.Pools.PoolModels
{
    public class CShakeMctPool : PoolBase<CShakeParameters, MctResult<CShakeResult>>
    {
        public CShakeMctPool(PoolConstructionParameters<CShakeParameters> param)
            : base(param) { }
    }
}
