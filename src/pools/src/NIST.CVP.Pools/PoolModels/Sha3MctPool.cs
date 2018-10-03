using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;

namespace NIST.CVP.Pools.PoolModels
{
    public class Sha3MctPool : PoolBase<Sha3Parameters, MctResult<HashResult>>
    {
        public Sha3MctPool(IOptions<PoolConfig> poolConfig, Sha3Parameters waterType, string filename, IList<JsonConverter> jsonConverters)
            : base(poolConfig, PoolTypes.SHA3_MCT, waterType, filename, jsonConverters) { }
    }
}
