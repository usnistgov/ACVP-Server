using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;

namespace NIST.CVP.Pools.PoolModels
{
    public class TdesMctPool : PoolBase<TdesParameters, MctResult<TdesResult>>
    {
        public TdesMctPool(TdesParameters waterType, string filename, IList<JsonConverter> jsonConverters)
            : base(PoolTypes.TDES_MCT, waterType, filename, jsonConverters) { }
    }
}
