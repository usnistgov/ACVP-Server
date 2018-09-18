using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;

namespace NIST.CVP.Pools.PoolModels
{
    public class Sha3MctPool : PoolBase<Sha3Parameters, MctResult<HashResult>>
    {
        public Sha3MctPool(Sha3Parameters waterType, string filename, IList<JsonConverter> jsonConverters)
            : base(PoolTypes.SHA3_MCT, waterType, filename, jsonConverters) { }
    }
}
