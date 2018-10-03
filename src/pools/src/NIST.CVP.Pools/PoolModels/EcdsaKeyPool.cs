using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;
using Microsoft.Extensions.Options;
using NIST.CVP.Common.Config;

namespace NIST.CVP.Pools.PoolModels
{
    public class EcdsaKeyPool : PoolBase<EcdsaKeyParameters, EcdsaKeyResult>
    {
        public EcdsaKeyPool(IOptions<PoolConfig> poolConfig, EcdsaKeyParameters waterType, string filename, IList<JsonConverter> jsonConverters)
            : base(poolConfig, PoolTypes.ECDSA_KEY, waterType, filename, jsonConverters) { }
    }
}
