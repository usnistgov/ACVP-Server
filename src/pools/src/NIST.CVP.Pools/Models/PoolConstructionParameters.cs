using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NIST.CVP.Common.Config;
using NIST.CVP.Common.Oracle;

namespace NIST.CVP.Pools.Models
{
    public class PoolConstructionParameters<TParam>
        where TParam : IParameters
    {
        public IOptions<PoolConfig> PoolConfig { get; set; }
        public TParam WaterType { get; set; }
        public IList<JsonConverter> JsonConverters { get; set; }
        public PoolProperties PoolProperties { get; set; }
        public string FullPoolLocation { get; set; }
    }
}
