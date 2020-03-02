using System.Threading.Tasks;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Models;

namespace NIST.CVP.Pools.PoolModels
{
    public class AesPool : PoolBase<AesParameters, AesResult>
    {
        public AesPool(PoolConstructionParameters<AesParameters> param)
            : base(param) { }

        public override async Task RequestWater()
        {
            await AddWater(await Oracle.GetAesCaseAsync(WaterType));
        }
    }
}
