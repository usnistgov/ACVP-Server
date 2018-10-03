using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;

namespace NIST.CVP.Pools.PoolModels
{
    public class ParallelHashMctPool : PoolBase<ParallelHashParameters, MctResult<ParallelHashResult>>
    {
        public ParallelHashMctPool(IOptions<PoolConfig> poolConfig, ParallelHashParameters waterType, string filename, IList<JsonConverter> jsonConverters)
            : base(poolConfig, PoolTypes.PARALLEL_HASH_MCT, waterType, filename, jsonConverters) { }
    }
}
