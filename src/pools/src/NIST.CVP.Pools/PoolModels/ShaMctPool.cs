using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Collections.Generic;

namespace NIST.CVP.Pools.PoolModels
{
    public class ShaMctPool : PoolBase<ShaParameters, MctResult<HashResult>>
    {
        public ShaMctPool(ShaParameters waterType, string filename, IList<JsonConverter> jsonConverters) : base(waterType, filename, jsonConverters) { }
    }
}
