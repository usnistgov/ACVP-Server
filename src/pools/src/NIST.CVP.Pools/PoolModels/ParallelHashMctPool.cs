using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Models;

namespace NIST.CVP.Pools.PoolModels
{
    public class ParallelHashMctPool : PoolBase<ParallelHashParameters, MctResult<ParallelHashResult>>
    {
        public ParallelHashMctPool(PoolConstructionParameters<ParallelHashParameters> param)
            : base(param) { }
    }
}
