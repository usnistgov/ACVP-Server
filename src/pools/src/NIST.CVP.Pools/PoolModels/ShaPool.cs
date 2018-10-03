using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;

namespace NIST.CVP.Pools.PoolModels
{
    public class ShaPool : PoolBase<ShaParameters, HashResult>
    {
        public ShaPool(IOptions<PoolConfig> poolConfig, ShaParameters waterType, string filename, IList<JsonConverter> jsonConverters)
            : base(poolConfig, PoolTypes.SHA, waterType, filename, jsonConverters) { }
    }
}
