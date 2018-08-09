using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using System.Collections.Generic;

namespace NIST.CVP.Pools.PoolTypes
{
    public class ShaPool : Pool<ShaParameters, HashResult>
    {
        public ShaPool(ShaParameters waterType, string filename, IList<JsonConverter> jsonConverters) : base(waterType, filename, jsonConverters) { }
    }
}
