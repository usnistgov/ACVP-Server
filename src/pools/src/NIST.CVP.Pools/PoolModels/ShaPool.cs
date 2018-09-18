using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;

namespace NIST.CVP.Pools.PoolModels
{
    public class ShaPool : PoolBase<ShaParameters, HashResult>
    {
        public ShaPool(ShaParameters waterType, string filename, IList<JsonConverter> jsonConverters)
            : base(PoolTypes.SHA, waterType, filename, jsonConverters) { }
    }
}
