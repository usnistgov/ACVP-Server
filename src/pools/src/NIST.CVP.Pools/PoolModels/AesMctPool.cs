using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;

namespace NIST.CVP.Pools.PoolModels
{
    public class AesMctPool : PoolBase<AesParameters, MctResult<AesResult>>
    {
        public AesMctPool(AesParameters waterType, string filename, IList<JsonConverter> jsonConverters)
            : base(PoolTypes.AES_MCT, waterType, filename, jsonConverters) { }
    }
}
