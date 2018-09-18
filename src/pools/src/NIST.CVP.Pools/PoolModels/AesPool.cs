using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;

namespace NIST.CVP.Pools.PoolModels
{
    public class AesPool : PoolBase<AesParameters, AesResult>
    {
        public AesPool(AesParameters waterType, string filename, IList<JsonConverter> jsonConverters) 
            : base(PoolTypes.AES, waterType, filename, jsonConverters) { }
    }
}
