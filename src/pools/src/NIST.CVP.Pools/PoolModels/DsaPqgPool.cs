using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Models;

namespace NIST.CVP.Pools.PoolModels
{
    public class DsaPqgPool : PoolBase<DsaDomainParametersParameters, DsaDomainParametersResult>
    {
        public DsaPqgPool(PoolConstructionParameters<DsaDomainParametersParameters> param)
            : base(param) { }
    }
}
