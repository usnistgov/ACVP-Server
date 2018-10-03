using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;

namespace NIST.CVP.Pools.PoolModels
{
    public class TupleHashMctPool : PoolBase<TupleHashParameters, MctResult<TupleHashResult>>
    {
        public TupleHashMctPool(IOptions<PoolConfig> poolConfig, TupleHashParameters waterType, string filename, IList<JsonConverter> jsonConverters)
            : base(poolConfig, PoolTypes.TUPLE_HASH_MCT, waterType, filename, jsonConverters) { }
    }
}
