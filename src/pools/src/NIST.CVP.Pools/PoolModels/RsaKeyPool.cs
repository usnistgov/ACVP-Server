using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;

namespace NIST.CVP.Pools.PoolModels
{
    public class RsaKeyPool : PoolBase<RsaKeyParameters, RsaPrimeResult>
    {
        public RsaKeyPool(RsaKeyParameters waterType, string filename, IList<JsonConverter> jsonConverters)
            : base(PoolTypes.RSA_KEY, waterType, filename, jsonConverters) { }
    }
}
