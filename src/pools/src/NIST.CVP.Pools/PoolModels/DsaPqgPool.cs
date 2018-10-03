using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;

namespace NIST.CVP.Pools.PoolModels
{
    public class DsaPqgPool : PoolBase<DsaDomainParametersParameters, DsaDomainParametersResult>
    {
        public DsaPqgPool(IOptions<PoolConfig> poolConfig, DsaDomainParametersParameters waterType, string filename, IList<JsonConverter> jsonConverters)
            : base(poolConfig, PoolTypes.DSA_PQG, waterType, filename, jsonConverters) { }
    }
}
