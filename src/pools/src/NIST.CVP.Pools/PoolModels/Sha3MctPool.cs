using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Models;

namespace NIST.CVP.Pools.PoolModels
{
    public class Sha3MctPool : PoolBase<Sha3Parameters, MctResult<HashResult>>
    {
        public Sha3MctPool(PoolConstructionParameters<Sha3Parameters> param)
            : base(param) { }
    }
}
