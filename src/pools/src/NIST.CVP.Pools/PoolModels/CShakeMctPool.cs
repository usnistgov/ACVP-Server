using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;

namespace NIST.CVP.Pools.PoolModels
{
    public class CShakeMctPool : PoolBase<CShakeParameters, MctResult<CShakeResult>>
    {
        public CShakeMctPool(CShakeParameters waterType, string filename, IList<JsonConverter> jsonConverters)
            : base(PoolTypes.CSHAKE_MCT, waterType, filename, jsonConverters) { }
    }
}
