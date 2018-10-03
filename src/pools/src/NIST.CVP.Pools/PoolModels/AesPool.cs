using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;

namespace NIST.CVP.Pools.PoolModels
{
    public class AesPool : PoolBase<AesParameters, AesResult>
    {
        public AesPool(IOptions<PoolConfig> poolConfig, AesParameters waterType, string filename, IList<JsonConverter> jsonConverters) 
            : base(poolConfig, PoolTypes.AES, waterType, filename, jsonConverters) { }
    }
}
