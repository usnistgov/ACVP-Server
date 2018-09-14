using Newtonsoft.Json;
using NIST.CVP.Common.Oracle.ParameterTypes;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;
using System.Collections.Generic;

namespace NIST.CVP.Pools.PoolModels
{
    public class DsaPqgPool : PoolBase<DsaDomainParametersParameters, DsaDomainParametersResult>
    {
        public DsaPqgPool(DsaDomainParametersParameters waterType, string filename, IList<JsonConverter> jsonConverters)
            : base(PoolTypes.DSA_PQG, waterType, filename, jsonConverters) { }
    }
}
