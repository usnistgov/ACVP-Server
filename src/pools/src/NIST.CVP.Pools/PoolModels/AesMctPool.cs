using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Models;

namespace NIST.CVP.Pools.PoolModels
{
    public class AesMctPool : PoolBase<AesParameters, MctResult<AesResult>>
    {
        public AesMctPool(PoolConstructionParameters<AesParameters> param)
            : base(param) { }

        public override async Task RequestWater()
        {
            AddWater(await Oracle.GetAesMctCaseAsync(WaterType));
        }
    }
}
