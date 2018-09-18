using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;

namespace NIST.CVP.Pools.PoolModels
{
    public class EcdsaKeyPool : PoolBase<EcdsaKeyParameters, EcdsaKeyResult>
    {
        public EcdsaKeyPool(EcdsaKeyParameters waterType, string filename, IList<JsonConverter> jsonConverters)
            : base(PoolTypes.ECDSA_KEY, waterType, filename, jsonConverters) { }
    }
}
