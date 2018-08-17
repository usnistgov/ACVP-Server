using Newtonsoft.Json;
using NIST.CVP.Common.Oracle;
using NIST.CVP.Common.Oracle.ResultTypes;
using NIST.CVP.Pools.Enums;

namespace NIST.CVP.Pools
{
    [JsonConverter(typeof(ParameterHolderConverter))]
    public class ParameterHolder
    {
        public PoolTypes Type { get; set; }
        public IParameters Parameters { get; set; }
        public IResult Result { get; set; }
    }
}
