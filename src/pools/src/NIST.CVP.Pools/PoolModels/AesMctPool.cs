using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;

namespace NIST.CVP.Pools.PoolModels
{
    public class AesMctPool : PoolBase<AesParameters, MctResult<AesResult>>
    {
        public AesMctPool(IOptions<PoolConfig> poolConfig, AesParameters waterType, string filename, IList<JsonConverter> jsonConverters)
            : base(poolConfig, PoolTypes.AES_MCT, waterType, filename, jsonConverters) { }
    }
}
