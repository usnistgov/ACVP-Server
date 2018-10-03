using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;

namespace NIST.CVP.Pools.PoolModels
{
    public class TdesMctPool : PoolBase<TdesParameters, MctResult<TdesResult>>
    {
        public TdesMctPool(IOptions<PoolConfig> poolConfig, TdesParameters waterType, string filename, IList<JsonConverter> jsonConverters)
            : base(poolConfig, PoolTypes.TDES_MCT, waterType, filename, jsonConverters) { }
    }
}
