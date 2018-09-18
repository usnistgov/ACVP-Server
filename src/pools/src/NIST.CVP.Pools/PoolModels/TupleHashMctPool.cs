using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;

namespace NIST.CVP.Pools.PoolModels
{
    public class TupleHashMctPool : PoolBase<TupleHashParameters, MctResult<TupleHashResult>>
    {
        public TupleHashMctPool(TupleHashParameters waterType, string filename, IList<JsonConverter> jsonConverters)
            : base(PoolTypes.TUPLE_HASH_MCT, waterType, filename, jsonConverters) { }
    }
}
