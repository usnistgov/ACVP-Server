using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;

namespace NIST.CVP.Pools.PoolModels
{
    public class ParallelHashMctPool : PoolBase<ParallelHashParameters, MctResult<ParallelHashResult>>
    {
        public ParallelHashMctPool(ParallelHashParameters waterType, string filename, IList<JsonConverter> jsonConverters)
            : base(PoolTypes.PARALLEL_HASH_MCT, waterType, filename, jsonConverters) { }
    }
}
